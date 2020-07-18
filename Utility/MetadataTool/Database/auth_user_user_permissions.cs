namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class auth_user_user_permissions
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int permission_id { get; set; }

        public virtual auth_permission auth_permission { get; set; }

        public virtual auth_user auth_user { get; set; }
    }
}
