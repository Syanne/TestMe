using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using System.Xml.Linq;
using Windows.UI.Xaml.Media;

namespace Test_me_alfa
{
    /// <summary>
    /// Data for choosen test
    /// </summary>
    public class TestInfo
    {
        public string path;             //path to xml-file with test
        public XDocument xDoc; 

        public int position;            //curent question
        public int qCount;              //number of questions in this test  
        public double result;           //result

        public byte minutes;            //if timer enabled - number of minutes for each question if this test
        public DispatcherTimer timer;   //timer
        public byte min, sec;           //minutes and seconds left

        public List<string> myAnswers;

        public TestInfo(string filePath, bool time)
        {
            path = filePath;
            xDoc = XDocument.Load(filePath);

            position = 1;
            qCount = Convert.ToInt32(xDoc.Root.Attribute("qcount").Value);
            result = 0;

            myAnswers = new List<string>();

            if (time)
            {
                minutes = Convert.ToByte(xDoc.Root.Attribute("minute").Value);
                min = minutes;
                sec = 0;

                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 1);
            }
        }
    }

    //Struct for chart
    struct Graph
    {
        public string Title { set; get; }
        public int Result { set; get; }
    }

    public class Member
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UniqueId { get; set; }
        public string Path { get; set; }
        public SolidColorBrush Background { get; set; }

        //set image
        public string _tImageSrc = "/Assets/logo300.scale-100.png";
        public string ImageSrc
        {
            get { return _tImageSrc; }
            set { _tImageSrc = value; }
        }
    }

    /// <summary>
    /// Group of tests
    /// </summary>
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public SolidColorBrush Foreground { get; set; }

        public ObservableCollection<Member> group { get; set; }

        public Group()
        {
            group = new ObservableCollection<Member>();
        }
    }

    public class SampleDataSource
    {
        public ObservableCollection<Group> grps = new ObservableCollection<Group>();

        /// <summary>
        /// add items to a group, then - return a group
        /// </summary>
        /// <param name="method"></param>
        public void AddGroup(int method)
        {
            SolidColorBrush dark = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 240, 240));
            SolidColorBrush light = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 250, 250, 250));
            int counter = 0;
            string name;
            switch (method)
            {
                case 1: name = "Психологические"; break;
                case 2: name = "Логика и IQ"; break;
                case 3: name = "Задачи и загадки"; break;
                default: name = "Error"; break;
            }

            XmlReader xml = XmlReader.Create("Data/MainSource.xml");
            Group feedGroup = new Group { GroupId = method, GroupName = name };
            while (xml.Read())
            {
                if (Convert.ToInt32(xml.GetAttribute("method")) == method)
                {
                    feedGroup.group.Add(new Member
                    {
                        UniqueId = Convert.ToInt32(xml.GetAttribute("id")),
                        Name = xml.GetAttribute("value").ToString(),
                        ImageSrc = xml.GetAttribute("img").ToString(),
                        Description = xml.GetAttribute("desc").ToString(),
                        Path = xml.GetAttribute("path"),
                        Background = (counter % 2 == 0) ? dark : light
                    });
                    counter +=1;
                }
            }
            grps.Add(feedGroup);
        }

        
        public void AddGroup(int method, int count)
        {
            XmlReader xml = XmlReader.Create("Data/MainSource.xml");
            Group feedGroup = new Group { GroupId = method };
            while (xml.Read())
            {
                if (feedGroup.group.Count != count)
                {
                    if (Convert.ToInt32(xml.GetAttribute("method")) == method)
                    {
                        feedGroup.group.Add(new Member
                        {
                            UniqueId = Convert.ToInt32(xml.GetAttribute("id")),
                            Name = xml.GetAttribute("value").ToString(),
                            ImageSrc = xml.GetAttribute("img").ToString(),
                            Path = xml.GetAttribute("path")
                        });
                    }
                }
                else break;
            }
            grps.Add(feedGroup);
        }

    }


    public class SampleGroupSource
    {
        public ObservableCollection<Member> CreateGroup(int method, ObservableCollection<Member> group)
        {
            group = new ObservableCollection<Member>();
            XmlReader xml = XmlReader.Create("Data/MainSource.xml");
            while (xml.Read())
            {
                if (Convert.ToInt32(xml.GetAttribute("method")) == method)
                {
                    group.Add(new Member
                    {
                        UniqueId = Convert.ToInt32(xml.GetAttribute("id")),
                        Name = xml.GetAttribute("value").ToString(),
                        ImageSrc = xml.GetAttribute("img").ToString(),
                        Description = xml.GetAttribute("desc").ToString(),
                        Path = xml.GetAttribute("path")
                    });
                }
            }

            return group;
        }

    }

}
