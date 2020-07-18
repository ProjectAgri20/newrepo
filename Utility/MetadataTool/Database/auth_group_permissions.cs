namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class auth_group_permissions
    {
        public int id { get; set; }

        public int group_id { get; set; }

        public int permission_id { get; set; }

        public virtual auth_group auth_group { get; set; }

        public virtual auth_permission auth_permission { get; set; }
    }
}
