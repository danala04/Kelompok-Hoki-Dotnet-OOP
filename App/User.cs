using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class User
    {
        private string name;
        private string role;
        private double saldo = 0;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        public double Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }

        public User(string name, string role)
        {
            this.name = name;
            this.role = role;
        }

        public User(string name, string role, double saldo)
        {
            this.name = name;
            this.role = role;
            this.saldo = saldo;
        }
    }
}
