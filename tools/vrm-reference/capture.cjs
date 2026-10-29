const { chromium } = require('playwright');
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');

async function run() {
    const fixture = path.resolve(process.argv[2] || '');
    if (!fixture.endsWith('.vrm') || !fs.existsSync(fixture) || fs.statSync(fixture).size > 64 * 1024 * 1024)
        throw new Error('Provide an existing local VRM, at most 64 MiB.');
    const root = path.resolve(__dirname, '../..');
    const output = path.join(root, 'UnityProjectIsekaid/LocalVrmEvidence/Presentation/reference');
    const cache = path.join(root, '.utmp/vrm-reference-verified');
    fs.mkdirSync(cache, { recursive: true });
    fs.mkdirSync(output, { recursive: true });
    const sources = require('./sources.json');
    for (const [name, source] of Object.entries(sources)) {
        const target = path.join(cache, name);
        const hash = data => crypto.createHash('sha256').update(data).digest('hex');
        if (fs.existsSync(target) && hash(fs.readFileSync(target)) === source.sha256) continue;
        const response = await fetch(source.url);
        if (!response.ok) throw new Error('Reference dependency download failed: ' + name);
        const bytes = Buffer.from(await response.arrayBuffer());
        if (hash(bytes) !== source.sha256) throw new Error('Reference dependency hash mismatch: ' + name);
        fs.writeFileSync(target, bytes);
    }
    const browser = await chromium.launch({ channel: 'msedge', headless: true,
        args: ['--enable-unsafe-swiftshader', '--use-angle=swiftshader', '--disable-background-networking'] });
    try {
        const page = await browser.newPage({ viewport: { width: 960, height: 1200 } });
        // Fulfill every model request from disk; the browser cannot send the avatar to a remote origin.
        await page.route('**/*', async route => {
            const url = new URL(route.request().url());
            if (url.origin !== 'http://isek.local') { await route.abort(); return; }
            if (url.pathname === '/capture' || url.pathname === '/result') {
                const capture = url.pathname === '/capture';
                const body = route.request().postData();
                if (!body || body.length > 12_000_000) throw new Error('Invalid local reference output');
                const target = path.join(output, capture ? 'three-vrm-reference.png' : 'three-vrm-reference.json');
                fs.writeFileSync(target, capture ? Buffer.from(body, 'base64') : body);
                await route.fulfill({ body: 'ok' }); return;
            }
            let file;
            if (url.pathname === '/avatar.vrm') file = fixture;
            else if (url.pathname === '/') file = path.join(__dirname, 'index.html');
            else {
                const name = path.basename(url.pathname);
                if (sources[name]) file = path.join(cache, name);
            }
            if (!file) { await route.fulfill({ status: 404, body: 'Missing local dependency' }); return; }
            await route.fulfill({ contentType: file.endsWith('.js') ? 'text/javascript' : file.endsWith('.html') ? 'text/html' : 'application/octet-stream', body: fs.readFileSync(file) });
        });
        await page.goto('http://isek.local/', { timeout: 60000 });
        await page.waitForFunction(() => document.querySelector('#status')?.textContent !== 'Loading local reference', { timeout: 60000 });
        const status = await page.locator('#status').textContent();
        if (!status.includes('loaded')) throw new Error(status);
        const results = JSON.parse(fs.readFileSync(path.join(output, 'three-vrm-reference.json'), 'utf8'));
        if (results.error || !results.materials.length || !fs.existsSync(path.join(output, 'three-vrm-reference.png'))) throw new Error('Incomplete reference result');
        console.log('ISEKAID_LOCAL_VRM_REFERENCE_PASSED');
    } finally { await browser.close(); }
}
run().catch(error => { console.error(error.message); process.exitCode = 1; });
