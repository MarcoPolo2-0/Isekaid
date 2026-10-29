using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Isekaid.ExperimentalVrm
{
    public static class WindowsAvatarPicker
    {
        public static string Pick()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            var dialog = new OpenFileName
            {
                size = Marshal.SizeOf<OpenFileName>(), owner = GetActiveWindow(),
                filter = "Avatar VRM 1.0 (*.vrm)\0*.vrm\0\0", file = new StringBuilder(32768), maxFile = 32768,
                title = "Seleccionar apariencia local · VRM 1.0 · máximo 32 MiB",
                flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008
            };
            if (GetOpenFileName(dialog)) return dialog.file.ToString();
            int error = CommDlgExtendedError();
            if (error != 0) throw new InvalidOperationException("Windows no pudo abrir el selector de archivos (" + error + ").");
            return null;
#else
            throw new PlatformNotSupportedException("La importación desde el selector está disponible en Windows.");
#endif
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private sealed class OpenFileName
        {
            public int size;
            public IntPtr owner, instance;
            public string filter;
            public IntPtr customFilter;
            public int maxCustomFilter, filterIndex;
            public StringBuilder file;
            public int maxFile;
            public IntPtr fileTitle;
            public int maxFileTitle;
            public string initialDirectory, title;
            public int flags;
            public short fileOffset, fileExtension;
            public string defaultExtension;
            public IntPtr customData, hook;
            public string templateName;
            public IntPtr reserved;
            public int reservedSize, flagsEx;
        }
        [DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName dialog);
        [DllImport("comdlg32.dll")] private static extern int CommDlgExtendedError();
        [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
#endif
    }
}
