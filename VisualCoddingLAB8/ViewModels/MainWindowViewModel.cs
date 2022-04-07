using VisualCoddingLAB8.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace VisualCoddingLAB8.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase __content;
        private static ObservableCollection<Kanban> __todoStates { get; set; }
        private static ObservableCollection<Kanban> __inprogressStates { get; set; }
        private static ObservableCollection<Kanban> __finishedStates { get; set; }
        public static ObservableCollection<Kanban> ToDoStates
        {
            set
            {
                __todoStates = value;
            }
            get => __todoStates;
        }
        public static ObservableCollection<Kanban> InProgressStates
        {
            set
            {
                __inprogressStates = value;
            }
            get => __inprogressStates;
        }
        public static ObservableCollection<Kanban> FinishedStates
        {
            set
            {
                __finishedStates = value;
            }
            get => __finishedStates;
        }
        public ViewModelBase Content
        {
            get => __content;
            private set => this.RaiseAndSetIfChanged(ref __content, value);
        }

        public void DeleteCompletedTask(Kanban x) => FinishedStates.Remove(x);
        public void DeleteInProgressTask(Kanban x)
        {
            InProgressStates.Remove(x);
            RefreshWindow();
        }
        public void DeleteToDoTask(Kanban x) => ToDoStates.Remove(x);
        public MainWindowViewModel()
        {
            __finishedStates = new ObservableCollection<Kanban>();
            __inprogressStates = new ObservableCollection<Kanban>();
            __todoStates = new ObservableCollection<Kanban>();
            RefreshWindow();
        }
        public void ReadDataFromFile(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ObservableCollection<Kanban>>));
                using (StreamReader reader = new StreamReader(path))
                {
                    FinishedStates.Clear();
                    InProgressStates.Clear();
                    ToDoStates.Clear();
                    List<ObservableCollection<Kanban>> states = (List<ObservableCollection<Kanban>>)serializer.Deserialize(reader);
                    ToDoStates = states[0];
                    InProgressStates = states[1];
                    FinishedStates = states[2];
                }
            }
            catch
            {
                return;
            }
        }
        public void WriteDataToFile(string path)
        {
            try
            {
                List<ObservableCollection<Kanban>> states = new List<ObservableCollection<Kanban>>();
                states.Add(ToDoStates);
                states.Add(InProgressStates);
                states.Add(FinishedStates);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<ObservableCollection<Kanban>>));
                    serializer.Serialize(writer, states);
                }
            }
            catch
            {
                return;
            }
        }
        public void NewTable()
        {
            FinishedStates.Clear();
            InProgressStates.Clear();
            ToDoStates.Clear();
        }
        public static void AddToDo() => ToDoStates.Add(new Kanban("Planned Task#" + (ToDoStates.Count + 1).ToString()));
        public static void AddDoing() => InProgressStates.Add(new Kanban("In Progress Task#" + (InProgressStates.Count + 1).ToString()));
        public static void AddDone() => FinishedStates.Add(new Kanban("Finished Task#" + (FinishedStates.Count + 1).ToString()));
        public void RefreshWindow() => Content = new StatusListModel();
    }
}
