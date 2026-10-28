from pathlib import Path
import json,hashlib
from PIL import Image,ImageDraw,ImageFont
p=Path(__file__).resolve().parents[1]
r=json.loads((p/'deformation-results.json').read_text())
font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',16)
views=('front','side','back','threequarter')
for view in views:
 sheet=Image.new('RGB',(2160,984),'#eeeeee');d=ImageDraw.Draw(sheet)
 for i,c in enumerate(r['combinations']):
  v=c['values_minus1_to_plus1'];code='_'.join(str(x+1) for x in v)
  im=Image.open(p/f'evidence/combinations/{code}_{view}.png');im.thumbnail((240,300));x=(i%9)*240;y=(i//9)*328
  sheet.paste(im,(x,y));d.text((x+5,y+303),f'P {v[0]:+} C {v[1]:+} W {v[2]:+}',font=font,fill='black')
 sheet.save(p/f'evidence/matrix-27-{view}.jpg',quality=95)
sheet=Image.new('RGB',(1440,952),'#eeeeee');d=ImageDraw.Draw(sheet)
for row,folder in enumerate((p.parent/'G2-PROTO',p)):
 for col,view in enumerate(views):
  im=Image.open(folder/f'evidence/neutral-body-{view}.png');im.thumbnail((360,450));sheet.paste(im,(col*360,row*476))
  d.text((col*360+5,row*476+453),('G2-PROTO' if row==0 else 'G2-CORR')+' / '+view,font=font,fill='black')
sheet.save(p/'evidence/before-after.jpg',quality=95)
for axis in ('Chest','Hips','Waist'):
 frames=[]
 for step in range(0,101,5):
  frame=Image.new('RGB',(1200,334),'#eeeeee');d=ImageDraw.Draw(frame)
  for context_index,context in enumerate(((0,0),(-1,-1),(1,1),(-1,1),(1,-1))):
   im=Image.open(p/f'evidence/sweeps/{axis}_{context_index}_{step:03}.png')
   frame.paste(im,(240*context_index,0));d.text((240*context_index+3,302),f'{axis} {-1+step*.02:+.1f} / {context}',font=font,fill='black')
  frames.append(frame)
 frames[0].save(p/f'evidence/sweep-{axis}.gif',save_all=True,append_images=frames[1:]+frames[-2:0:-1],duration=100,loop=0)
manifest={str(f.relative_to(p)).replace(chr(92),'/'):hashlib.sha256(f.read_bytes()).hexdigest() for f in p.rglob('*') if f.is_file() and f.suffix!='.blend1' and f.name!='artifact-manifest.json'}
(p/'artifact-manifest.json').write_text(json.dumps(manifest,indent=2))
print('CORR_EVIDENCE_PACKAGED')
