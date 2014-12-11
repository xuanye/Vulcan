namespace Vulcan.DataAccess.ORMapping.MSSql
{
    public class MSSqlRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSSqlRepository"/> class.
        /// </summary>
        /// <param name="constr">The constr.</param>
        public MSSqlRepository(string constr)
            : base(constr)
        {
        }
    }
}