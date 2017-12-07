using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public class Submission
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }

        public string dateOfBirth { get; set; }
        public string serial { get; set; }

        public Submission()
        {

        }
        public Submission(string fn, string ln, string em, string pn, string dob, string s)
        {
            this.firstName = fn;
            this.lastName = ln;
            this.email = em;
            this.phoneNumber = pn;
            this.dateOfBirth = dob;
            this.serial = s;
        }
        override
        public string ToString()
        {
            string delimiter = "|";
            return firstName + delimiter + lastName + delimiter + email + delimiter + phoneNumber + delimiter + dateOfBirth + delimiter + serial;
        }
    }
}
