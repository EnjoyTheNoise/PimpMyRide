namespace PimpMyRide.Core.Cars.Dto
{
    public class GetCarByIdResponse
    {
        public string ModelName { get; set; }

        public string Manufacturer { get; set; }

        public string EngineType { get; set; }

        public double EngineCapacity { get; set; }

        public double PriceForDay { get; set; }

        public double PriceForHour { get; set; }

        public double Mileage { get; set; }

        public double Collateral { get; set; }
    }
}
