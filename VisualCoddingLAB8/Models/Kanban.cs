using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Imaging;
namespace VisualCoddingLAB8.Models
{
    [Serializable]
    public class Kanban : INotifyPropertyChanged
    {
        private string __name = "";
        private string __stateText = "";
        private Bitmap? __pathToFile;
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;
        [XmlIgnore]
        public Bitmap? PathToFile
        {
            get => __pathToFile;
            set
            {
                if (value != null)
                {
                    __pathToFile = value;
                    RaisePropertyChangedEvent("PathToFile");
                }
            }
        }
        public string Name
        {
            set
            {
                if (value != null)
                {
                    __name = value;
                    RaisePropertyChangedEvent("Name");
                }
            }
            get => __name;
        }
        public string StateText
        {
            set
            {
                if (value != null)
                {
                    __stateText = value;
                    RaisePropertyChangedEvent("StateText");
                }
            }
            get => __stateText;
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("PathToFile")]
        public byte[] SerializedImage
        {
            get
            {
                if (PathToFile == null)
                {
                    return null;
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    PathToFile.Save(stream);
                    return stream.ToArray();
                }
            }
            set
            {
                if (value == null)
                {
                    PathToFile = null;
                }
                else
                {
                    using (MemoryStream stream = new MemoryStream(value))
                    {
                        PathToFile = new Bitmap(stream);
                    }
                }
            }
        }

        public Kanban(string name)
        {
            Name = name;
            StateText = "";
            PathToFile = null;
        }
        public Kanban()
        {
            Name = "";
            StateText = "";
            PathToFile = null;
        }
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
        public async void GetImage(Window x)
        {
            try
            {
                var openDialog = new OpenFileDialog().ShowAsync(x);
                string[]? path = await openDialog;
                if (path != null)
                {
                    string srcPath = string.Join(@"/", path);
                    FileInfo fInfo = new FileInfo(srcPath);
                    using (FileStream reader = fInfo.OpenRead())
                    {
                        try
                        {
                            PathToFile = Bitmap.DecodeToWidth(reader, 100);
                        }
                        catch
                        {
                            PathToFile = null;
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}
