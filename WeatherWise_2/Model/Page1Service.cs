using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWise_2.Core.Models;

namespace WeatherWise_2.Model
{
    public class Page1Service
    {
        private Person _myPerson;

        public string WelcomeString { get; set; }

        public Person MyPerson
        {
            get { return _myPerson; }
            set { _myPerson = value; }
        }

        public Page1Service()
        {
            
            MyPerson = new Person();
        }

        public Person InitializePage()
        {
            MyPerson = new Person() { FirstName = "Jimbo", LastName = "Smith" };
            WelcomeString ="You are Welcome";
            _myPerson.FirstName = "Bubba";
            _myPerson.LastName = "Jones";
            return MyPerson;
        }

    }
    
}
