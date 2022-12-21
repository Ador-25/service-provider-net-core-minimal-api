using System.ComponentModel.DataAnnotations;

namespace ServiceProviderApi.Models
{

    public class ServiceProvider
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Key]
        public int ServiceProviderID { get; set; }

        public string Email { get; set; }

        public string NID { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public double Lon { get; set; }

        public double Lat { get; set; }
        public TypeofEmployees ServiceType { get; set; }
        public double DistanceFrom(GoogleLocation temp)
        {
            double lon1 = toRadians(this.Lon);
            double lon2 = toRadians(temp.tempLon);
            double lat1 = toRadians(this.Lat);
            double lat2 = toRadians(temp.tempLat);
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in
            // kilometers. Use 3956
            // for miles
            double r = 6371;

            // calculate the result
            return (c * r);

        }
        private double toRadians(
           double angleIn10thofaDegree)
        {
            // Angle in 10th
            // of a degree
            return (angleIn10thofaDegree *
                           Math.PI) / 180;
        }


    }
    public enum TypeofEmployees
    {
        Electrician = 200,
        Technician = 200,
        Plumber = 200,
        DeliveryMan = 100,
        Driver = 1000,
        Carpenter = 200,
        Mechanic = 200,
    }
}
