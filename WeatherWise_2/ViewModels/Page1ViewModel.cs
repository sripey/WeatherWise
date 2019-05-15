using System;

using GalaSoft.MvvmLight;
using WeatherWise_2.Core.Models;
using WeatherWise_2.Model;

namespace WeatherWise_2.ViewModels
{
    public class Page1ViewModel : ViewModelBase
    {


        private string _myText;
        public Page1Service Page1ServiceData { get; set; }

        public string MyText
        {
            get { return _myText; }
            set { _myText = value; }
        }

        private Person _thisPerson;

        public Person ThisPerson
        {
            get { return _thisPerson; }
            set { _thisPerson = value; }
        }






        public Page1ViewModel()
        {

            Page1ServiceData = new Page1Service();
            // _page1Service = new Page1Service();
            ThisPerson = new Person();
            ThisPerson = Page1ServiceData.MyPerson;
            Page1ServiceData.MyPerson = new Person();
            ThisPerson = Page1ServiceData.MyPerson;
            MyText = Page1ServiceData.WelcomeString;
            

        }

        public  void InitializePage()
        {

            //_page1Service = new Page1Service();
            Page1ServiceData = new Page1Service();
            ThisPerson = new Person();
            Page1ServiceData.MyPerson = new Person();

            ThisPerson = Page1ServiceData.MyPerson;
            MyText = Page1ServiceData.WelcomeString;
            Page1ServiceData.InitializePage();
            
            
        }

    }
}
