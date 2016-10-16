namespace TimetableSolver.Samples.Models
{
    public class Penalties
    {
        public int TeacherCollisionPenalty { get; set; }
        public int TeacherWindowPenalty { get; set; }
        public int ClassCollisionPenalty { get; set; }
        public int ClassWindowPenalty { get; set; }
        public int ClassFrontWindowPenalty { get; set; }

        public static Penalties DefaultPenalties()
        {
            return new Penalties
            {
                ClassCollisionPenalty = 100,
                ClassFrontWindowPenalty =1,
                ClassWindowPenalty = 4,
                TeacherCollisionPenalty = 100,
                TeacherWindowPenalty = 4
            };
        }
    }
}
