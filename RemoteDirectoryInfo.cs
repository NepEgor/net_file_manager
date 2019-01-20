using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directories
{
    public class RemoteFileInfo
    {
        public string Name { get; }
        public string FullName { get; }
        public string Path { get; }

        public RemoteDirectoryInfo Parent { get; }
        public RemoteDirectoryInfo Root { get; }
        
        public RemoteFileInfo(string path, RemoteDirectoryInfo parent, RemoteDirectoryInfo root)
        {
            Path = path;

            int pos = path.LastIndexOf('\\');
            if (pos != -1) Name = path.Substring(pos + 1);
            else Name = Path;

            Parent = parent;
            Root = root;

            FullName = Parent.FullName + '\\' + Name;
        }
        
        public RemoteFileInfo(FileInfo file, RemoteDirectoryInfo parent, RemoteDirectoryInfo root)
        {
            Path = file.FullName;
            Name = file.Name;
            FullName = parent.FullName + '\\' + Name;
            
            Root = root;
            Parent = parent;
        }
    }

    public class RemoteDirectoryInfo
    {
        public string Name { get; }
        public string FullName { get; }
        public string Path { get; }

        public RemoteDirectoryInfo Parent { get; }
        public RemoteDirectoryInfo Root { get; }

        public List<RemoteDirectoryInfo> Directories { get; }
        public List<RemoteFileInfo> Files { get; }

        public RemoteDirectoryInfo(string path, RemoteDirectoryInfo parent = null, RemoteDirectoryInfo root = null)
        {
            Path = path;
            
            int pos = path.LastIndexOf('\\');
            if(pos != -1) Name = path.Substring(pos + 1);
            else          Name = Path;
            
            Parent = parent;
            if (root == null)
            {
                Root = this;
                Path = null;
                FullName = Name;
            }
            else
            {
                Root = root;
                FullName = Parent.FullName + '\\' + Name;
            }

            Directories = new List<RemoteDirectoryInfo>();
            Files = new List<RemoteFileInfo>();

        }

        public RemoteDirectoryInfo(DirectoryInfo dir, RemoteDirectoryInfo parent, RemoteDirectoryInfo root)
        {
            Path = dir.FullName;
            Name = dir.Name;
            FullName = parent.FullName + '\\' + Name;

            Directories = new List<RemoteDirectoryInfo>();
            Files = new List<RemoteFileInfo>();

            Root = root;
            Parent = parent;
        }

        public void AddDirectory(string name)
        {
            string path = FullName + '\\' + name;
            Directories.Add(new RemoteDirectoryInfo(path, this, Root));
        }
        public void AddFile(string name)
        {
            string path = FullName + '\\' + name;
            Files.Add(new RemoteFileInfo(path, this, Root));
        }

        private void UpdateLocalContent()
        {
            if (Path == null) return;
            DirectoryInfo di = new DirectoryInfo(Path);
            Directories.Clear();
            Files.Clear();
            foreach(var dir in di.GetDirectories())
            {
                Directories.Add(new RemoteDirectoryInfo(dir, this, Root));
            }
            foreach (var file in di.GetFiles())
            {
                Files.Add(new RemoteFileInfo(file, this, Root));
            }
        }

        public byte[] ToBytes()
        {
            UpdateLocalContent();

            int data_len = 4 + FullName.Length * 2 + 8;
            byte[] data;

            foreach (var dir in Directories)
            {
                data_len += 4 + dir.Name.Length * 2; // Unicode is 2 bytes/char
            }
            foreach (var file in Files)
            {
                data_len += 4 + file.Name.Length * 2;
            }
            
            data = new byte[data_len];
            int i = 0;

            byte[] full_name_len = BitConverter.GetBytes(FullName.Length);
            byte[] full_name = Encoding.Unicode.GetBytes(FullName);
            for (int j = 0; j < full_name_len.Length; ++j, ++i)
            {
                data[i] = full_name_len[j];
            }
            for (int j = 0; j < full_name.Length; ++j, ++i)
            {
                data[i] = full_name[j];
            }
            
            byte[] dir_num = BitConverter.GetBytes(Directories.Count);
            for (int j = 0; j < dir_num.Length; ++j, ++i)
            {
                data[i] = dir_num[j];
            }

            foreach (var dir in Directories)
            {
                byte[] name_len = BitConverter.GetBytes(dir.Name.Length);
                byte[] name = Encoding.Unicode.GetBytes(dir.Name);
                for (int j = 0; j < name_len.Length; ++j, ++i)
                {
                    data[i] = name_len[j];
                }
                for (int j = 0; j < name.Length; ++j, ++i)
                {
                    data[i] = name[j];
                }
            }
            
            byte[] file_num = BitConverter.GetBytes(Files.Count);
            for (int j = 0; j < file_num.Length; ++j, ++i)
            {
                data[i] = file_num[j];
            }

            foreach(var file in Files)
            {
                byte[] name_len = BitConverter.GetBytes(file.Name.Length);
                byte[] name = Encoding.Unicode.GetBytes(file.Name);
                for (int j = 0; j < name_len.Length; ++j, ++i)
                {
                    data[i] = name_len[j];
                }
                for (int j = 0; j < name.Length; ++j, ++i)
                {
                    data[i] = name[j];
                }
            }

            return data;
        }

        public static RemoteDirectoryInfo FromBytes(byte[] bytes)
        {
            string path;
            string[] dirs;
            string[] files;

            int i = 0;

            {
                int len = BitConverter.ToInt32(bytes, i) * 2;

                i += 4;
                path = Encoding.Unicode.GetString(bytes, i, len);

                i += len;
            }

            int num_dirs = BitConverter.ToInt32(bytes, i);
            i += 4;
            dirs = new string[num_dirs];

            for(int j = 0; j < num_dirs; ++j)
            {
                int len = BitConverter.ToInt32(bytes, i) * 2;

                i += 4;
                dirs[j] = Encoding.Unicode.GetString(bytes, i, len);

                i += len;
            }

            int num_files = BitConverter.ToInt32(bytes, i);
            i += 4;
            files = new string[num_files];

            for (int j = 0; j < num_files; ++j)
            {
                int len = BitConverter.ToInt32(bytes, i) * 2;

                i += 4;
                files[j] = Encoding.Unicode.GetString(bytes, i, len);

                i += len;
            }

            RemoteDirectoryInfo directory = (RemoteDirectoryInfo)OnPath(Program.main_form.remote_root, path);
            directory.Directories.Clear();
            directory.Files.Clear();
            foreach (string dir in dirs)
            {
                Console.Write("Added directory ");
                Console.WriteLine(dir);
                directory.AddDirectory(dir);
            }
            foreach (string file in files)
            {
                Console.Write("Added file ");
                Console.WriteLine(file);
                directory.AddFile(file);
            }

            return directory;
        }
        
        public void Update(RemoteDirectoryInfo upd_dir)
        {
            
            if (Root.FullName != upd_dir.Root.FullName || Parent.FullName != upd_dir.Parent.FullName) return;

            Directories.Clear();
            Files.Clear();
            foreach (var dir in upd_dir.Directories) Directories.Add(dir);
            foreach (var file in upd_dir.Files) Files.Add(file);

            /*
            foreach (var dir in Directories)
            {
                int count = 0;
                foreach (var u_dir in upd_dir.Directories)
                {
                    if (dir.Name == u_dir.Name)
                    {
                        break;
                    }
                    else ++count;
                }

                if (count >= upd_dir.Directories.Count)
                {

                }
            }
            */
        }

        public static object OnPath(RemoteDirectoryInfo root, string path)
        {
            Console.WriteLine(path);
            Console.WriteLine(root.Name);

            object obj = null;

            int i = path.IndexOf(root.Name);
            Console.WriteLine(i);
            string sub_path;
            if (i == 0)
            {
                int j = path.IndexOf('\\');
                if (j == root.Name.Length)
                {
                    sub_path = path.Remove(0, root.Name.Length + 1);
                    foreach (var dir in root.Directories)
                    {
                        obj = OnPath(dir, sub_path);
                        if (obj != null) return obj;
                    }

                    foreach (var file in root.Files)
                    {
                        if (sub_path == file.Name)
                        {
                            Console.WriteLine("Found file");
                            return file;
                        }
                    }
                }
                else
                {
                    if (path == root.Name)
                    {
                        Console.WriteLine("Found directory");
                        return root;
                    }
                }
            }

            Console.Write("No such file or directory: ");
            Console.WriteLine(path);
            return null;
        }
        
    }
}
