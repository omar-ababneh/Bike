using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[Serializable]
class SYSTEM
{
    /*    int CustomerLength;
    */
    Bike[] bikes = new Bike[10000];
    Customer[] customers = new Customer[5];
    Rental_Record[] RR = new Rental_Record[100000];
    int BikeLength;
    int RentalRecordsLength;
    int CustomerLength;
    public SYSTEM()
    { //* constructing for bike
        FileStream fs;
        BinaryFormatter bf;
        int j = 0;
        CustomerLength = 5;
        fs = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        bf = new BinaryFormatter();
        while (fs.Position < fs.Length)
        {
            bikes[j] = (Bike)bf.Deserialize(fs);
            j++;
        }
        BikeLength = j;
        fs.Close();
        //* constructing for RentalRecords
        fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        int RentalCount = 0;
        while (fs.Position < fs.Length)
        {
            RR[RentalCount] = (Rental_Record)bf.Deserialize(fs);
            RentalCount++;
        }
        RentalRecordsLength = RentalCount;
        fs.Close();
        //*constructing for Customers
        /* fs = new FileStream("Customers.txt", FileMode.OpenOrCreate, FileAccess.Read);
        int CustomersCount = 0;
        while (fs.Position < fs.Length)
            customers[CustomersCount++] = (Customer)bf.Deserialize(fs);
        fs.Close();
        CustomerLength = 5;*/

    }
    public void UpdateBikeRental(string QR, string RentalID, string ParkID)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;
        fs = new FileStream("RentalRecord.txt", FileMode.Create, FileAccess.Write);
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            if (RR[i].GetRentalID() == RentalID)
                RR[i].SetStatus("on-rent");
            bf.Serialize(fs, RR[i]);
        }
        fs.Close();
        fs = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        bf = new BinaryFormatter();
        j = 0;
        while (fs.Position < fs.Length)
            bikes[j++] = (Bike)bf.Deserialize(fs);
        fs.Close();
        BikeLength = j;
        fs = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QR)
            {
                bikes[i].setStatus("Locked");
                bikes[i].setParkID(ParkID);
            }
            bf.Serialize(fs, bikes[i]);
        }
        fs.Close();
        fs = new FileStream("RentalRecord.txt", FileMode.Create, FileAccess.Write);
        bf = new BinaryFormatter();
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            if (RR[i].GetRentalID() == RentalID)
                RR[i].SetStatus("ended");
            bf.Serialize(fs, RR[i]);
        }
        fs.Close();

    }

    public string GetQRCode(string RentalID)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;

        for (int i = 0; i < RentalRecordsLength; i++)
            if (RR[i].GetRentalID() == RentalID)
                return RR[i].GetQRCode();
        return "BK10";
    }
    public void UpdateBikeReview(int review, string QR)
    {
        FileStream fs;
        BinaryFormatter bf;
        fs = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            bikes[j++] = (Bike)bf.Deserialize(fs);
        fs.Close();
        BikeLength = j;
        fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        bf = new BinaryFormatter();
        j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;
        int count = 0;
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            if (RR[i].GetStatus() == "ended" && RR[i].GetQRCode() == QR)
                count++;
        }
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QR)
            {
                bikes[i].setReview((review + bikes[i].getReview()) / 2);
                    }
                }
            
        
        this.SaveAndUpdateBikes();

    }
    public int ViewAllRentedRecords(string name)
    {
        int count = 0;
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
        {
            RR[j] = (Rental_Record)bf.Deserialize(fs);
            j++;
        }
        fs.Close();
        RentalRecordsLength = j;
        if (RentalRecordsLength == 0)
            return count;

        for (int i = 0; i < RentalRecordsLength; i++)
        {
            if (RR[i].GetCustomerName() == name && RR[i].GetStatus() == "on-rent")
            {
                RR[i].Display();
                count++;
            }
        }
        return count;

    }
    public string getQR(string RentalID)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;
        for (int i = 0; i < RentalRecordsLength; i++)
            if (RR[i].GetRentalID() == RentalID)
            {
                return RR[i].GetQRCode();
            }
        return "BK1";

    }
    public void ChangeStatusBikeRental(string QR, string RentalID)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;
        fs = new FileStream("RentalRecord.txt", FileMode.Create, FileAccess.Write);
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            if (RR[i].GetRentalID() == RentalID)
                RR[i].SetStatus("on-rent");
            bf.Serialize(fs, RR[i]);
        }
        fs.Close();
        fs = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        bf = new BinaryFormatter();
        j = 0;
        while (fs.Position < fs.Length)
            bikes[j++] = (Bike)bf.Deserialize(fs);
        fs.Close();
        BikeLength = j;
        fs = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QR)
                bikes[i].setStatus("unlocked");
            bf.Serialize(fs, bikes[i]);
        }
        fs.Close();

    }
    public int DisplayAllInActiveRR(string name)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        RentalRecordsLength = j;
        fs.Close();
        if (RentalRecordsLength == 0)
        { Console.WriteLine("No Rental Records found"); return 0; }
        int count = 0;
        for (int i = 0; i < RentalRecordsLength; i++)
            if (RR[i].GetStatus() == "inactive" && RR[i].GetCustomerName() == name)
            {
                RR[i].Display();
                count++;

            }
        if (count == 0)
            Console.WriteLine("No InActive Rental Records found");

        return count;
    }
    public void ViewEndedRR(string name)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        RentalRecordsLength = j;
        fs.Close();

        if (RentalRecordsLength == 0)
            Console.WriteLine("No rental records to show");
        else
        {
            for (int i = 0; i < RentalRecordsLength; i++)
            {
                if (RR[i].GetCustomerName() == name && RR[i].GetStatus() == "ended")
                    RR[i].Display();

            }
        }
    }

    public int GetBaseRate(string QR)
    {
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QR)
                return bikes[i].getBase_rate();
        }
        return 0;
    }
    public void DisplayCustomers()
    {
        for (int i = 0; i < CustomerLength; i++)
        {
            Console.WriteLine(customers[i].getName());
            Console.WriteLine(customers[i].getPassword());
            Console.WriteLine(customers[i].getCash_Credit());
        }
    }
    public void DisplayRentalRecords()
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
        {
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        }
        fs.Close();
        RentalRecordsLength = j;

        if (RentalRecordsLength == 0)
            Console.WriteLine("No rental records found");
        else
        {
            for (int i = 0; i < RentalRecordsLength; i++)
                RR[i].Display();
        }
    }
    public void DeleteBike(string QR)
    {
        FileStream fs;
        BinaryFormatter bf;
        bf = new BinaryFormatter();
        bool flag = false;
        int index = 0;
        for (int i = 0; i < BikeLength; i++)
            if (bikes[i].getStatus() == "Locked" && bikes[i].getQR() == QR)
            {
                index = i;
                flag = true;

            }
        if (flag == true)
        {
            FileStream FS;
            BinaryFormatter BF;
            BF = new BinaryFormatter();
            FS = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
            for (int i = 0; i < BikeLength; i++)
            {
                if (index == i)
                    continue;
                bf.Serialize(FS, bikes[i]);
            }
            BikeLength--;
            FS.Close();
            UpdateBikeList();
            Console.WriteLine("Bike successfuly deleted");
        }
        /*
        else
        {
          FileStream fs = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
            fs.Close();
        }*/


        if (flag == false)
            Console.WriteLine("Bike not found");
    }
    public void ChargeCredit(string name, double cash)
    {
        FileStream fs = new FileStream("Customers.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            customers[j++] = (Customer)bf.Deserialize(fs);

        fs.Close();
        fs = new FileStream("Customers.txt", FileMode.Create, FileAccess.Write);
        bf = new BinaryFormatter();
        for (int i = 0; i < 5; i++)
        {
            if (customers[i].getName() == name)
                customers[i].setCash_Credit(cash);
            bf.Serialize(fs, customers[i]);
        }
        fs.Close();
    }
    public void UpdateBikeList()
    {
        FileStream fs;
        BinaryFormatter bf;
        bf = new BinaryFormatter();
        fs = new FileStream("Bikes.txt", FileMode.Open, FileAccess.Read);
        int j = 0;
        while (fs.Position < fs.Length)
        {
            bikes[j++] = (Bike)bf.Deserialize(fs);
        }
        BikeLength = j;
        fs.Close();
    }
    public void SetBikeLength(int length)
    {
        BikeLength = length;
    }
    public int GetBikeLength()
    {
        return BikeLength;
    }
    public void AddNewBike(string QR_Code, string Type, string ParkID, string Status, int Review, int Base_rate)
    {
        BinaryFormatter bf;
        bf = new BinaryFormatter();
        bool flag = false;
        FileStream fs1 = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        fs1.Position = 0;
        while (fs1.Position < fs1.Length)
        {
            Bike bike = (Bike)bf.Deserialize(fs1);
            if (bike.getQR() == QR_Code)
            {
                flag = true;
                break;

            }
        }
        fs1.Close();

        if (flag == true)
        {
            Console.WriteLine("Bike not added, you must provide a unique QR Code");
        }
        if (flag == false)
        {
            bikes[BikeLength] = new Bike(QR_Code, Type, ParkID, Status, Review, Base_rate);
            FileStream fs2 = new FileStream("Bikes.txt", FileMode.Append, FileAccess.Write);
            bf.Serialize(fs2, bikes[BikeLength]);
            fs2.Close();
            BikeLength++;
            Console.WriteLine("Bike added succefully");

        }

    }
    public void ViewBikeInfo()
    {
        bool t = false;
        if (BikeLength == 0)
            Console.WriteLine("There are no bikes to display information for!");

        else
            for (int i = 0; i < BikeLength; i++)
            {
                if(t==false)
                Console.WriteLine("QR Code" + "   " + "Park ID" + "   " + "Base Rate" + "   " + "Review" + "   " + "Status" + "   " + "Type");
                Console.WriteLine();
                Console.WriteLine(bikes[i].getQR() + "       " + bikes[i].getParkID() + "       " + bikes[i].getBase_rate() + "          " + bikes[i].getReview() + "%" + "        " + bikes[i].getStatus() + "   " + bikes[i].getType());
                Console.WriteLine();
                t = true;
            }
    }
    // This also saves the rental record into the data base and changes status
    public void CreateRentalRecordStatus(string RentalID, string CustomerName, string QR_Code, int Duration, int Day, int Month, int Year, double RentalAmount)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int d= 0;
        while (fs.Position < fs.Length)
        {
            RR[d] = (Rental_Record)bf.Deserialize(fs);
            d++;
        }
        fs.Close();
        RentalRecordsLength = d;
        int j = RentalRecordsLength;
        RR[RentalRecordsLength++] = new Rental_Record();
        RR[j].SetCustomerName(CustomerName);
        RR[j].setRentalID(RentalID);
        RR[j].SetQR_Code(QR_Code);
        RR[j].SetDuartion(Duration);
        RR[j].SetStatus("inactive");
        RR[j].SetRentalPayment(CustomerName, RentalAmount, Day, Month, Year);
         fs = new FileStream("RentalRecord.txt", FileMode.Append, FileAccess.Write);
         bf = new BinaryFormatter();
        bf.Serialize(fs, RR[j]);
        fs.Close();
    }
    public int ValidateQRCode(string QR, string ParkID)
    {
        for (int j = 0; j < BikeLength; j++)

            if (bikes[j].getParkID() == ParkID && bikes[j].getQR() == QR && bikes[j].getStatus() == "Locked")
                return j;

        return -1;
    }
    public int ViewLockedBikes(string ParkID)
    {
        FileStream fs = new FileStream("Bikes.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
           while(fs.Position<fs.Length)
            bikes[j++] = (Bike)bf.Deserialize(fs);
        fs.Close();
        BikeLength = j;
        int count = 0;
        if (BikeLength == 0)
        {
            Console.WriteLine("There are no bikes to display information for!");
            return -1;
        }
        else
            for (int i = 0; i < BikeLength; i++)
            {
                if (bikes[i].getStatus() == "Locked" && bikes[i].getParkID() == ParkID)
                {
                    Console.WriteLine("QR Code" + "   " + "Park ID" + "   " + "Base Rate" + "   " + "Review" + "   " + "Status" + "   " + "Type");
                    Console.WriteLine();
                    Console.WriteLine(bikes[i].getQR() + "       " + bikes[i].getParkID() + "       " + bikes[i].getBase_rate() + "          " + bikes[i].getReview() + "%" + "        " + bikes[i].getStatus() + "   " + bikes[i].getType());
                    Console.WriteLine();
                    count++;
                }
            }
        if (count == 0)
        {
            Console.WriteLine("There are no bikes available at your location");
            return -1;
        }

        return 1;
    }
    public int GetRRLength()
    {
        return this.RentalRecordsLength;
    }
    public void DisplayRentalPayements()
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            Console.WriteLine("Customer Name \t Rental Start Date \t Rental Amount");
            Console.WriteLine(RR[i].GetCustomerName() + "\t \t \t " + RR[i].GetRentalPayment().GetStartDate().GetDay() + "/" + RR[i].GetRentalPayment().GetStartDate().GetMonth() + "/" + RR[i].GetRentalPayment().GetStartDate().GetYear() + "\t \t" + RR[i].GetRentalPayment().GetRentalAmount());
        }

    }
    public void ChangeBikeStatus(string QRCode)
    {
        FileStream fs = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QRCode && bikes[i].getStatus() == "Locked")
            {
                bikes[i].setStatus("Reserved");
            }


            bf.Serialize(fs, bikes[i]);

        }
        fs.Close();
        UpdateBikeList();

    }
    public Rental_Record GetRentalRecord(string RentalID)
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        fs.Close();
        RentalRecordsLength = j;

        for (int i = 0; i > RentalRecordsLength; i++)
        {
            if (RR[i].GetRentalID() == RentalID)
            {
                return RR[i];
            }
        }
        return null;
    }
    public Bike GetBike(string QR)
    {
        FileStream fs = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            bikes[j++] = (Bike)bf.Deserialize(fs);
        fs.Close();
        BikeLength = j;
        for (int i = 0; i < BikeLength; i++)
        {
            if (bikes[i].getQR() == QR)
            {
                return bikes[i];
            }
        }
        return null;
    }
    public bool ValidateRentalID(string RentalID, int x, string name)
    {

        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
            RR[j++] = (Rental_Record)bf.Deserialize(fs);
        RentalRecordsLength = j;
        fs.Close();
        if (x == 0)
        {
            for (int i = 0; i < RentalRecordsLength; i++)
            {
                if (RR[i].GetRentalID() == RentalID)
                {
                    return false;
                }

            }
            return true;
        }
        if (x == 1)
        {
            for (int i = 0; i < RentalRecordsLength; i++)
            {
                if (RR[i].GetStatus() == "inactive" && RR[i].GetCustomerName() == name)
                    return true;
            }
            return false;
        }
        if (x == -1)
        {
            for (int i = 0; i < RentalRecordsLength; i++)
            {
                if (RR[i].GetStatus() == "on-rent" && RR[i].GetRentalID() == RentalID)
                    return true;
            }
            return false;
        }
        return false;

    }
    public void CalculateProfit()
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int counter = 0;
        while (fs.Position < fs.Length)
        {
            RR[counter] = (Rental_Record)bf.Deserialize(fs);
            counter++;
        }
        fs.Close();
        RentalRecordsLength = counter;
        if (GetRRLength() == 0)
        {
            Console.WriteLine("No Rental Payments to be displayed");
            return;
        }
        else
        {
            DisplayRentalPayements();
            Date d1, d2;
            Console.WriteLine("Enter day of Start Date : ");
            int d1d = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter month of Start Date : ");
            int d1m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter year of Start Date : ");
            int d1y = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter day of End Date : ");
            int d2d = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter month of End Date : ");
            int d2m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter year of End Date : ");
            int d2y = Convert.ToInt32(Console.ReadLine());

            DateTime Date1 = new DateTime(d1y, d1m, d1d);
            DateTime Date2 = new DateTime(d2y, d2m, d2d);
            DateTime Date3;
            double TotalRentalAmount = 0;

            TimeSpan check = Date1 - Date2;
            if (check.Days > 0)
            {
                Console.WriteLine("Incorrect Dates Entered,Try Again.");
                return;
            }
            else
            {
                if (check.Days == 0)
                {
                    for (int i = 0; i < RentalRecordsLength; i++)
                    {
                        Date3 = new DateTime(RR[i].GetRentalPayment().GetStartDate().GetYear(), RR[i].GetRentalPayment().GetStartDate().GetMonth(), RR[i].GetRentalPayment().GetStartDate().GetDay());
                        if (Date3 == Date1 && Date3 == Date2)
                        {
                            TotalRentalAmount += RR[i].GetRentalPayment().GetRentalAmount();
                        }
                    }
                }
                else {
                    for (int i = 0; i < RentalRecordsLength; i++)
                    {
                        Date3 = new DateTime(RR[i].GetRentalPayment().GetStartDate().GetYear(), RR[i].GetRentalPayment().GetStartDate().GetMonth(), RR[i].GetRentalPayment().GetStartDate().GetDay());
                        if (Date3 >= Date1 && Date3 <= Date2)
                        {
                            TimeSpan t = Date2 - Date3;
                                double f=t.Days*(RR[i].GetRentalPayment().GetRentalAmount());
                            TotalRentalAmount +=f;
                        }
                    }
                   
                }
                
            }
            Console.WriteLine("Total Profit in the given period : " + TotalRentalAmount);
            return;
        }
    }
    public void SaveAndUpdateBikes()
    {
        FileStream fs = new FileStream("Bikes.txt", FileMode.Create, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        for (int i = 0; i < BikeLength; i++)
        {
            bf.Serialize(fs, bikes[i]);
        }
        int ind = 0;
        BikeLength = 0;
        while (fs.Position < fs.Length)
        {
            bikes[ind] = (Bike)bf.Deserialize(fs);
            ind++;
        }
        BikeLength = ind;
        fs.Close();
    }
    public void SaveAndUpdateCustomers(string name,double cash_credit) {
        FileStream fs = new FileStream("Customers.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        while (fs.Position < fs.Length)
        {
            customers[j++] = (Customer)bf.Deserialize(fs);

        }
        fs.Close();
        CustomerLength = 5;
         fs = new FileStream("Customers.txt", FileMode.Create, FileAccess.Write);

        for (int i = 0; i < 5; i++)
        {
            if (customers[i].getName() == name)
            {
                customers[i].setCash_Credit(cash_credit);
            }
            bf.Serialize(fs, customers[i]);

        }
        fs.Close();
      
    }
    public void SaveAndUpdateRR()
    {
        FileStream fs = new FileStream("RentalRecord.txt", FileMode.Create, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        for (int i = 0; i < RentalRecordsLength; i++)
        {
            bf.Serialize(fs, RR[i]);
        }
       /* int ind = 0;
        RentalRecordsLength = 0;
        fs.Position = 0;
        while (fs.Position < fs.Length)
        {
            RR[ind] = (Rental_Record)bf.Deserialize(fs);
            ind++;
        }
        RentalRecordsLength = ind;*/
    }

}
[Serializable]
class Operator
{
    SYSTEM s;
    string Name;
    string Password;
    public Operator(string Name, string Password)
    {
        if (Name == "admin" && Password == "00")
        {
            this.Name = Name;
            this.Password = Password;
        }
        s = new SYSTEM();
    }
    public void setName(string Name)
    {
        this.Name = Name;
    }
    public string getName()
    {
        return this.Name;
    }
    public void setPassword(string Password)
    {
        this.Password = Password;
    }
    public string getPassword()
    {
        return this.Password;
    }
    public void AddNewBike()
    {
        Console.WriteLine("Please enter the QR Code");
        string QR = Console.ReadLine();
        Console.WriteLine("Please enter the bike's type");
        string Type = Console.ReadLine();
        Console.WriteLine("please enter the park ID");
        string ParkID = Console.ReadLine();
        Console.WriteLine("Please enter the base rate");
        string br = Console.ReadLine();
        int Base_rate = Convert.ToInt32(br);
        s.AddNewBike(QR, Type, ParkID, "Locked", 0, Base_rate);


    }
    public void RemoveLockedBike()
    {
        FileStream fs3 = new FileStream("Bikes.txt", FileMode.OpenOrCreate, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int count = 0;

        while (fs3.Position < fs3.Length)
        {
            Bike bike = (Bike)bf.Deserialize(fs3);

            if (bike.getStatus() == "Locked")
            {
                Console.WriteLine("QR Code" + "   " + "Park ID" + "   " + "Base Rate" + "   " + "Review" + "            " + "Status" + "   " + "Type");
                Console.WriteLine();
                Console.WriteLine(bike.getQR() + "       " + bike.getParkID() + "       " + bike.getBase_rate() + "          " + bike.getReview() + "%" + "   " + bike.getStatus() + "   " + bike.getType());
                Console.WriteLine();
                count++;

                /*
                // -->another way to display information
                Console.WriteLine("Bike's QR Code   : " + bike[j].getQR());
                Console.WriteLine("Bike's ParkID    : " + bike[j].getParkID());
                Console.WriteLine("Bike's Base Rate : " + bike[j].getBase_rate());
                Console.WriteLine("Bike's Review    : " + bike[j].getReview());
                Console.WriteLine("Bike's Status    : " + bike[j].getStatus());
                Console.WriteLine("Bike's Type      : " + bike[j].getType());
                Console.WriteLine();
                count++;*/
            }
        }
        fs3.Close();
        if (count == 0)
            Console.WriteLine("No bikes found");
        else
        {
            Console.Write("Please enter the QR Code of the bike you want to delete");
            string QR = Console.ReadLine();
            s.DeleteBike(QR);
        }

    }
    public void ViewBikesInformation()
    {
        s.ViewBikeInfo();
    }
    public void ViewRR()
    {
        s.DisplayRentalRecords();
    }
    public void CalculateProfit()
    {
        s.CalculateProfit();

    }
    //if (d1y > d2y)
    //{
    //    Console.WriteLine("Invalid Dates Entered,try again");
    //    return;
    //}
    //else if (d1y == d2y && d1m > d2m) {
    //    Console.WriteLine("Invalid Dates Entered,try again");
    //    return;
    //}
    //else if (d1y == d2y && d1m == d2m && d1d > d2d)
    //{
    //    Console.WriteLine("Invalid Dates Entered,try again");
    //    return;
    //}


}
[Serializable]
class Customer
{

    SYSTEM s = new SYSTEM();
    string Name;
    string Password;
    double Cash_Credit;
    public Customer()
    {
        s = new SYSTEM();
    }

    public void setName(string Name)
    {
        this.Name = Name;
    }
    public void DisplayCashCredit()
    {
        Console.WriteLine(this.Cash_Credit);
    }
    public string getName()
    {
        return this.Name;
    }
    public void setPassword(string Password)
    {
        this.Password = Password;
    }
    public string getPassword()
    {
        return this.Password;
    }
    public void setCash_Credit(double Cash_Credit)
    {
        this.Cash_Credit = Cash_Credit;
    }
    public double getCash_Credit()
    {
        return this.Cash_Credit;
    }

    public void ReserveBike()
    {
        Console.WriteLine("Please Enter your current location");
        string Location = Console.ReadLine();
        string ParkID;
        if (Location == "Zone A" || Location == "Zone B")
            ParkID = "Park 1";
        else
            if (Location == "Zone C" || Location == "Zone D")
            ParkID = "Park 2";
        else
                if (Location == "Zone E" || Location == "Zone F")
            ParkID = "Park 3";
        else { Console.WriteLine("Invalid Location"); return; }

        int flag = s.ViewLockedBikes(ParkID);
        if (flag == -1)
            return;
        else
        {
            Console.WriteLine("Enter the QR Code from the list above");
            string QRCode = Console.ReadLine();
            flag = s.ValidateQRCode(QRCode, ParkID);
            if (flag == -1)
            {
                Console.WriteLine("Invalid input");
                return;
            }
            int d1d, d1m, d1y;
            int d2d, d2m, d2y;

            Console.WriteLine("Please enter the start date");
            Console.WriteLine("Enter day");
            d1d = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter month");
            d1m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter year");
            d1y = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the end date");
            Console.WriteLine("Enter day");
            d2d = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter month");
            d2m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter year");
            d2y = Convert.ToInt32(Console.ReadLine());

            DateTime Date1 = new DateTime(d1y, d1m, d1d);
            DateTime Date2 = new DateTime(d2y, d2m, d2d);

            TimeSpan check = Date2 - Date1;
            if (check.Days < 0)
            {
                Console.WriteLine("Incorrect Dates Entered,Try Again.");
                return;
            }
            int duration = check.Days;
            Console.WriteLine("Enter the Rental ID");
            string RentalID = Console.ReadLine();
            bool isUnique = s.ValidateRentalID(RentalID, 0, "");
            if (isUnique == false)
            {
                Console.WriteLine("RentalID already exists");
                return;
            }

            int baserate;
            baserate = s.GetBaseRate(QRCode);
            double RentalAmount = 0;
            if (check.Days < 3)
                RentalAmount += baserate;
            if (check.Days > 3 && check.Days <= 10)
                RentalAmount += baserate * 0.7;
            if (check.Days > 10)
                RentalAmount += baserate * 0.4;


            RentalAmount = RentalAmount * duration;
            if (this.Cash_Credit < RentalAmount)
            {

                Console.WriteLine("Insufficient funds");
                return;
            }
            double x = this.Cash_Credit - RentalAmount;
            this.Cash_Credit = x;
         /*   BinaryFormatter bf = new BinaryFormatter;
            FileStream fs = new FileStream("Customers.txt", FileMode.OpenOrCreate, FileAccess.wr);
            int j = 0;*/

            s.SaveAndUpdateCustomers(this.Name,x);
            s.ChangeBikeStatus(QRCode);
            s.CreateRentalRecordStatus(RentalID, this.getName(), QRCode, duration, d1d, d1m, d1y, x);

        }
    }
    public void UnlockBike()
    {
        int flag = s.DisplayAllInActiveRR(this.Name);
        if (flag == 0)
            return;
        Console.WriteLine("Please write the Rental ID from the list above");
        string RentalID = Console.ReadLine();
        bool flag2 = s.ValidateRentalID(RentalID, 1, this.Name);
        if (flag2 == false)
        {
            Console.WriteLine("Rental ID not in list");
            return;
        }
        if (flag2 == true)
        {
            string QR = s.getQR(RentalID);

            /*Rental_Record RREdit = s.GetRentalRecord(RentalID);
            Bike BEdit =  s.GetBike(RREdit.GetQRCode());
            RREdit.SetStatus("on-rent");
            BEdit.setStatus("unlocked");
            s.SaveAndUpdateBikes();
            s.SaveAndUpdateRR();*/
            s.ChangeStatusBikeRental(QR, RentalID);

        }


    }
    public void ReturnBike()
    {
        int x = s.ViewAllRentedRecords(this.Name);
        if (x == 0)
        {
            Console.WriteLine("No rental records found");
            return;
        }
        Console.WriteLine("Please enter 1 of the REntal IDS from above");
        string RentalID = Console.ReadLine();
        bool flag = s.ValidateRentalID(RentalID, -1, this.Name);
        if (flag == false)
        { Console.WriteLine("Type a correct Rental ID"); return; }
        Console.WriteLine("Please  enter your review out of a 100 for the returned bike");

        int review = Convert.ToInt32(Console.ReadLine());
        string QR = s.getQR(RentalID);
        s.UpdateBikeReview(review, QR);
        string ParkID = "";
        Console.WriteLine("Please enter the location where you want to return the bike");
        string Location = Console.ReadLine();
        if (Location == "Zone A" || Location == "Zone B")
            ParkID = "Park 1";
        else
            if (Location == "Zone C" || Location == "Zone D")
            ParkID = "Park 2";
        else
                if (Location == "Zone E" || Location == "Zone F")
            ParkID = "Park 3";
        else
        {
            Console.WriteLine("Wrong Location, try again");
            return;
        }

        s.UpdateBikeRental(QR, RentalID, ParkID);


    }
    public void ViewEndedRR()
    {
        s.ViewEndedRR(this.getName());
    }
    public void ChargeCashCredit()
    {
        Console.WriteLine("Please enter the amount of cash to feed your credit");
        double cash = Convert.ToDouble(Console.ReadLine());
        if (cash <= 0)
            Console.WriteLine("Please put a real amount");
        else
        {
            this.Cash_Credit = this.Cash_Credit + cash;
            s.ChargeCredit(this.Name, this.Cash_Credit);
            this.DisplayCashCredit();

        }
    }
}
[Serializable]
class Bike
{
    string QR_Code;
    string Type;
    string ParkID;
    string Status;
    int Review;
    int Base_rate;
    public Bike(string QR_Code, string Type, string ParkID, string Status, int Review, int Base_rate)
    {
        this.QR_Code = QR_Code;
        this.Type = Type;
        this.ParkID = ParkID;
        this.Status = Status;
        this.Review = Review;
        this.Base_rate = Base_rate;
    }
    public void setQR(string QR_Code)
    {
        this.QR_Code = QR_Code;
    }
    public string getQR()
    {
        return this.QR_Code;
    }
    public void setType(string Type)
    {
        this.Type = Type;
    }
    public string getType()
    {
        return this.Type;
    }
    public void setParkID(string ParkID)
    {
        this.ParkID = ParkID;
    }
    public string getParkID()
    {
        return this.ParkID;
    }
    public void setStatus(string Status)
    {
        this.Status = Status;
    }
    public string getStatus()
    {
        return this.Status;
    }
    public void setReview(int Review)
    {
        this.Review = Review;
    }
    public int getReview()
    {
        return this.Review;
    }
    public void setBase_rate(int Base_rate)
    {
        this.Base_rate = Base_rate;
    }
    public int getBase_rate()
    {
        return this.Base_rate;
    }
}
[Serializable]
class Rental_Record
{
    string Rental_ID;
    string CustomerName;
    string QR_Code;
    int Duartion;
    string Status;
    Rental_Payment RP;
    public void setRentalID(string Rental_ID)
    {
        this.Rental_ID = Rental_ID;

    }
    public void SetCustomerName(string CustomerName)
    {
        this.CustomerName = CustomerName;

    }
    public void SetQR_Code(string QR_Code)
    {
        this.QR_Code = QR_Code;

    }
    public void SetDuartion(int Duration)
    {
        this.Duartion = Duration;

    }
    public void SetStatus(string Status)
    {
        this.Status = Status;

    }
    public void SetRentalPayment(string CustomerName, double RentalAmount, int day, int month, int year)
    {

        RP = new Rental_Payment(CustomerName, RentalAmount, day, month, year);
    }
    public string GetRentalID()
    {
        return this.Rental_ID;
    }
    public string GetCustomerName()
    {
        return this.CustomerName;
    }
    public string GetQRCode()
    {
        return this.QR_Code;
    }
    public int GetDuration()
    {
        return this.Duartion;
    }
    public string GetStatus()
    {
        return this.Status;
    }
    public Rental_Payment GetRentalPayment()
    {
        return this.RP;
    }
    public void Display()
    {
        Console.WriteLine("Rental ID \t Customer Name \t QR Code \t Duartion \t Status \t Rental Start Date \t Rental Amount");
        Console.WriteLine(this.Rental_ID    + "\t" +   this.CustomerName + "\t" +      this.QR_Code + "\t" +        this.Duartion + "\t" +       this.Status + "\t" +       this.RP.GetStartDate().GetDay() + "/" +          this.RP.GetStartDate().GetMonth() + "/" +             this.RP.GetStartDate().GetYear() + "\t" +           this.RP.GetRentalAmount());

    }

}
[Serializable]
class Rental_Payment
{
    Date StartDate;
    string CustomerName;
    double RentalAmount;
    public Rental_Payment(string CustomerName, double RentalAmount, int day, int month, int year)
    {
        this.CustomerName = CustomerName;
        this.RentalAmount = RentalAmount;
        StartDate = new Date(day, month, year);

    }
    public Date GetStartDate()
    {
        return this.StartDate;

    }
    public double GetRentalAmount()
    {
        return this.RentalAmount;
    }
    public string GetCustomerName()
    {
        return this.CustomerName;
    }

}
[Serializable]
class Date
{
    int day, month, year;
    public Date(int day, int month, int year)
    {
        this.day = day;
        this.month = month;
        this.year = year;
    }
    public int GetDay()
    {
        return this.day;
    }
    public int GetMonth()
    {
        return this.month;
    }
    public int GetYear()
    {
        return this.year;

    }
}
[Serializable]
class Program
{
    static void DisplayMainMenue()
    {
        Console.WriteLine("[1] Login as Operator");
        Console.WriteLine("[2] Login as Customer");
        Console.WriteLine("[3] Exit");
        Console.WriteLine("");
        Console.WriteLine("Enter your choice: ");
    }
    static void DisplayOperatorMenue()
    {
        Console.WriteLine("[1] Add new bike to the fleet ");
        Console.WriteLine("[2] Remove locked bike from the fleet ");
        Console.WriteLine("[3] View full information of all bikes ");
        Console.WriteLine("[4] View all rental records for all bikes ");
        Console.WriteLine("[5] Calculate profit for a specefic period ");
        Console.WriteLine("[6] Back to login screen ");
        Console.WriteLine("");
        Console.WriteLine("Enter your choice: ");


    }
    static void DisplayCustomerMenu(Customer c)
    {
        Console.WriteLine("Welcome, " + c.getName() + "\n");
        Console.WriteLine("[1] Reserve a bike");
        Console.WriteLine("[2] Unlock a bike");
        Console.WriteLine("[3] Return a bike");
        Console.WriteLine("[4] View all ended rental records");
        Console.WriteLine("[5] Charge cash credit");
        Console.WriteLine("[6] Back to login screen\n");
        Console.WriteLine("Enter your choice: ");
    }
    static int ValidateCustomer(string un, string p)
    {
        FileStream fs = new FileStream("Customers.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        int j = 0;
        Customer[] C = new Customer[5];
        while (fs.Position < fs.Length)
        {
            C[j] = (Customer)bf.Deserialize(fs);
            if (C[j].getName() == un && C[j].getPassword() == p)
            {
                fs.Close();
                return j;
            }
            j++;
        }


        fs.Close();
        return -1;

    }
    static void Main()
    {
        /*Customer[] c = new Customer[5];
        c[0] = new Customer();
        c[1] = new Customer();
        c[2] = new Customer();
        c[3] = new Customer();
        c[4] = new Customer();

        c[0].setName("Ali");
        c[0].setPassword("11");
        c[0].setCash_Credit(150);
        c[1].setName("Omar");
        c[1].setPassword("22");
        c[1].setCash_Credit(140);
        c[2].setName("Maha");
        c[2].setPassword("33");
        c[2].setCash_Credit(95);
        c[3].setName("Hamzah");
        c[3].setPassword("44");
        c[3].setCash_Credit(50);
        c[4].setName("Reem");
        c[4].setPassword("44");
        c[4].setCash_Credit(120);
        FileStream fs = new FileStream("Customers.txt", FileMode.Create, FileAccess.Write);
        int j = 0;
        BinaryFormatter bf = new BinaryFormatter();
        for (int i = 0; i < 5; i++)
            bf.Serialize(fs, c[i]);
        fs.Close();
        Bike[] b = new Bike[5];
        b[0] = new Bike("BK1", "Sport", "Park 1", "Locked", 67, 20);
        b[1] = new Bike("BK2", "Road", "Park 2", "Locked", 75, 15);
        b[2] = new Bike("BK3", "Mountain", "Park 2", "Locked", 90, 35);
        b[3] = new Bike("BK4", "Sport", "Park 3", "Locked", 53, 25);
        b[4] = new Bike("BK5", "Road", "Park 1", "Locked", 88, 10);
         bf = new BinaryFormatter();
        FileStream fs1 = new FileStream("Bikes.txt", FileMode.Open, FileAccess.Write);
        for (int i = 0; i < 5; i++)
            bf.Serialize(fs1, b[i]);
        fs1.Close();*/


        FileStream fs1 = new FileStream("Customers.txt", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf1 = new BinaryFormatter();
        int j = 0;
        Customer[] C = new Customer[5];
        while (fs1.Position < fs1.Length)
            C[j++] = (Customer)bf1.Deserialize(fs1);

        fs1.Close();



        string ChoiceOP; string ChoiceCU;
        DisplayMainMenue();
        string choice = Console.ReadLine();
        string username; string password;
        while (choice != "3")
        {
            if (choice == "1")
            {
                Console.WriteLine("Enter username: ");
                username = Console.ReadLine();
                Console.WriteLine("Enter password: ");
                password = Console.ReadLine();
                if (username == "admin" && password == "00")
                {
                    Console.WriteLine("Welcome, operator");
                    Console.WriteLine("");
                    DisplayOperatorMenue();

                    Operator OP = new Operator("admin", "00");
                    ChoiceOP = Console.ReadLine();
                    while (ChoiceOP != "6")
                    {
                        if (ChoiceOP == "1")
                            OP.AddNewBike();
                        if (ChoiceOP == "2")
                            OP.RemoveLockedBike();
                        if (ChoiceOP == "3")
                            OP.ViewBikesInformation();
                        if (ChoiceOP == "4")
                            OP.ViewRR();
                        if (ChoiceOP == "5")
                            OP.CalculateProfit();
                        DisplayOperatorMenue();
                        ChoiceOP = Console.ReadLine();
                    }
                    DisplayMainMenue();
                    choice = Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Login not successful, try again.");
                    DisplayMainMenue();
                    choice = Console.ReadLine();

                }

            }
            if (choice == "2")
            {
                Console.WriteLine("Enter username: ");
                username = Console.ReadLine();
                Console.WriteLine("Enter password: ");
                password = Console.ReadLine();
                int v = ValidateCustomer(username, password);
                if (v == -1)
                {
                    Console.WriteLine("Wrong username or password");

                    continue;
                }
                else DisplayCustomerMenu(C[v]);
                ChoiceCU = Console.ReadLine();
                while (ChoiceCU != "6")
                {

                    if (ChoiceCU == "1")
                        C[v].ReserveBike();
                    if (ChoiceCU == "2")
                        C[v].UnlockBike();
                    if (ChoiceCU == "3")
                        C[v].ReturnBike();
                    if (ChoiceCU == "4")
                        C[v].ViewEndedRR();
                    if (ChoiceCU == "5")
                    {
                        C[v].ChargeCashCredit();


                    }
                    DisplayCustomerMenu(C[v]);
                    ChoiceCU = Console.ReadLine();

                }
                DisplayMainMenue();
                choice = Console.ReadLine();
            }
        }
    }
}


