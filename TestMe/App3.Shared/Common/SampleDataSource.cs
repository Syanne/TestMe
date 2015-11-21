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

namespace Test_me_alfa
{
    public struct HelpStruct
    {
        public string path;             //path to xml-file with test

        public int position;            //curent question
        public int numOfq;              //number of questions in this test  
        public double result;           //result

        public byte minutes;            //if timer enabled - number of minutes for each question if this test
        public DispatcherTimer timer;   //timer
        public byte min, sec;           //minutes and seconds left
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
            XmlReader xml = XmlReader.Create("Data/MainSource.xml");
            Group feedGroup = new Group { GroupId = method };
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
                        Path = xml.GetAttribute("path")
                    });
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
