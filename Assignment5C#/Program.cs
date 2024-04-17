using System;
using System.Collections.Generic;

namespace PostOffice
{
    class Program
    {
        // Define the Main method, the entry point of the program
        static void Main(string[] args)
        {
            // Create a new MailBox object with a capacity limit of 30
            MailBox mailBox = new MailBox(30);

            // Create some mail objects: letters, advertisements, and parcels
            Letter letter1 = new Letter(200, true, "123 Main St, Anytown", "A3");
            Letter letter2 = new Letter(800, false, "", "A4");

            Advertisement ad1 = new Advertisement(1500, true, "456 Elm St, Othertown");
            Advertisement ad2 = new Advertisement(3000, false, "");

            Package package1 = new Package(5000, true, "789 Oak St, Anycity", 30);
            Package package2 = new Package(3000, true, "321 Maple Ave, Someville", 70);

            // Add the mail objects to the mail box
            mailBox.AddMail(letter1);
            mailBox.AddMail(letter2);
            mailBox.AddMail(ad1);
            mailBox.AddMail(ad2);
            mailBox.AddMail(package1);
            mailBox.AddMail(package2);

            // Display the total postage cost for all mail in the mail box
            Console.WriteLine($"The total postage cost is {mailBox.CalculatePostage():0.00}");

            // Display details of each mail in the mail box
            mailBox.DisplayMail();

            // Display the number of invalid mails in the mail box
            Console.WriteLine("The mail box contains " + mailBox.InvalidMailCount() + " invalid mails");

            // Wait for user input before closing the console window
            Console.ReadKey();
        }
    }

    // Define an abstract class called Mail
    abstract class Mail
    {
        // Define properties for weight, express delivery status, and destination address
        public double Weight { get; set; }
        public bool IsExpress { get; set; }
        public string DestinationAddress { get; set; }

        // Constructor for Mail class
        public Mail(double weight, bool isExpress, string destinationAddress)
        {
            Weight = weight;
            IsExpress = isExpress;
            DestinationAddress = destinationAddress;
        }

        // Abstract methods that must be implemented by derived classes
        public abstract double CalculatePostage();
        public abstract bool IsValid();
        public abstract string GetMailType();
    }

    // Define a class called Letter that inherits from Mail
    class Letter : Mail
    {
        // Define a property for the letter format
        public string Format { get; }

        // Constructor for Letter class
        public Letter(double weight, bool isExpress, string destinationAddress, string format)
            : base(weight, isExpress, destinationAddress)
        {
            Format = format;
        }

        // Implement the abstract methods from the Mail class
        public override double CalculatePostage()
        {
            double postage = 0.0;
            if (IsExpress)
            {
                postage = Format == "A3" ? 7.0 + Weight * 2.0 : 5.0 + Weight * 2.0;
            }
            else
            {
                postage = Format == "A3" ? 3.5 + Weight : 2.5 + Weight;
            }
            return postage / 1000;
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DestinationAddress);
        }

        public override string GetMailType()
        {
            return "Letter";
        }
    }

    // Define a class called Package that inherits from Mail
    class Package : Mail
    {
        // Define a property for the package volume
        public double Volume { get; }

        // Constructor for Package class
        public Package(double weight, bool isExpress, string destinationAddress, double volume)
            : base(weight, isExpress, destinationAddress)
        {
            Volume = volume;
        }

        // Implement the abstract methods from the Mail class
        public override double CalculatePostage()
        {
            double postage = 0.0;
            if (IsExpress)
            {
                postage = 2.0 * Volume + Weight * 2.0;
            }
            else
            {
                postage = Volume * 0.25 + Weight;
            }
            return postage / 1000;
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DestinationAddress) && Volume <= 50.0;
        }

        public override string GetMailType()
        {
            return "Package";
        }
    }

    // Define a class called Advertisement that inherits from Mail
    class Advertisement : Mail
    {
        // Constructor for Advertisement class
        public Advertisement(double weight, bool isExpress, string destinationAddress)
            : base(weight, isExpress, destinationAddress)
        {
        }

        // Implement the abstract methods from the Mail class
        public override double CalculatePostage()
        {
            double postage = IsExpress ? Weight * 10.0 : Weight * 5.0;
            return postage / 1000;
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DestinationAddress);
        }

        public override string GetMailType()
        {
            return "Advertisement";
        }
    }

    // Define a class called MailBox
    class MailBox
    {
        // Define a list to store mail objects and a capacity limit for the mail box
        private List<Mail> mails = new List<Mail>();
        private int Capacity { get; }

        // Constructor for MailBox class
        public MailBox(int capacity)
        {
            Capacity = capacity;
        }

        // Method to add mail to the mail box
        public void AddMail(Mail mail)
        {
            if (mails.Count < Capacity)
            {
                mails.Add(mail);
            }
        }

        // Method to calculate the total postage cost for all mail in the mail box
        public double CalculatePostage()
        {
            double totalPostage = 0.0;
            foreach (var mail in mails)
            {
                totalPostage += mail.CalculatePostage();
            }
            return totalPostage;
        }

        // Method to count the number of invalid mails in the mail box
        public int InvalidMailCount()
        {
            int count = 0;
            foreach (var mail in mails)
            {
                if (!mail.IsValid())
                {
                    count++;
                }
            }
            return count;
        }

        // Method to display details of each mail in the mail box
        public void DisplayMail()
        {
            foreach (var mail in mails)
            {
                Console.WriteLine($"{mail.GetMailType()}");

                if (mail.IsValid())
                {
                    Console.WriteLine($"Weight: {mail.Weight: 0.0} grams");
                    Console.WriteLine($"Express: {(mail.IsExpress ? "yes" : "no")}");
                    Console.WriteLine($"Destination: {mail.DestinationAddress}");
                    Console.WriteLine($"Price: $ {(mail.CalculatePostage()):0.00}");
                }
                else
                {
                    Console.WriteLine("Invalid mail");
                    Console.WriteLine($"Weight: {mail.Weight: 0.0} grams");
                    Console.WriteLine($"Express: {(mail.IsExpress ? "yes" : "no")}");
                    Console.WriteLine($"Destination: {mail.DestinationAddress}");
                    Console.WriteLine($"Price: $ 0.00");
                }

                if (mail is Letter letter)
                {
                    Console.WriteLine($"Format: {letter.Format}");
                }
                else if (mail is Package package)
                {
                    Console.WriteLine($"Volume: {package.Volume: 0.0} liters");
                }

                Console.WriteLine();
            }
        }
    }
}
