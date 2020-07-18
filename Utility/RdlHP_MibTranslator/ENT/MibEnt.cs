
namespace HP.RDL.RdlHPMibTranslator
{
    public class MibEnt
    {
        public string Title { get; set; }
        public string Parent { get; set; }
        public int Value { get; set; }

        public MibEnt()
        {
            Title = string.Empty;
            Parent = string.Empty;
            Value = 0;
        }
        public override bool Equals(object obj)
        {
            var temp = obj as MibEnt;
            bool bEquals = false;

            if(obj != null)
            {
                if(this.Title.ToLower().Equals(temp.Title.ToLower()))
                {
                    bEquals = true;
                    if(string.IsNullOrEmpty(this.Parent))
                    {
                        this.Parent = temp.Parent;
                    }
                }
            }

            return bEquals;
        }
    }
}
