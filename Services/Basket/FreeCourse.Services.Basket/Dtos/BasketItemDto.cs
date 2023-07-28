namespace FreeCourse.Services.Basket.Dtos
{
    public class BasketItemDto
    {
        public int Quantity { get; set; } // aslında 1 kurstan daha fazla alınmaz ama e-ticaret örnekli olsun
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price{ get; set; }
    }
}
