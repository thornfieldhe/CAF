namespace CAF.Security
{
    public class CAFRoles : SingletonBase<CAFRoles>
    {
        //private Database db = CAFCache.Instance.MembershipServices;

        //public void AddUsersToRole(string[] usernames, string roleName)
        //{
        //    Roles.AddUsersToRole(usernames, roleName);
        //}
        //public void AddUsersToRoles(string[] usernames, string[] roleNames)
        //{
        //    Roles.AddUsersToRoles(usernames, roleNames);
        //}
        //public void AddUserToRole(string username, string roleName)
        //{
        //    Roles.AddUserToRole(username, roleName);
        //}
        //public void AddUserToRoles(string username, string[] roleNames)
        //{
        //    Roles.AddUserToRoles(username, roleNames);
        //}

        ///// <summary>
        ///// 创建角色
        ///// CustomRoleType=0 角色
        ///// CustomRoleType=1 用户组
        ///// </summary>
        ///// <param name="roleName"></param>
        ///// <param name="roleType"></param>
        //public void CreateRole(string roleName, CustomRoleType roleType)
        //{
        //    db.ExecuteNonQuery("aspnet_Roles_CreateCAFRole", Roles.ApplicationName, roleName, (int)roleType);
        //}

        //public void DeleteCookie()
        //{
        //    Roles.DeleteCookie();
        //}

        //public void DeleteRole(string roleName, bool throwOnPopulatedRole)
        //{
        //    Roles.DeleteRole(roleName, throwOnPopulatedRole);
        //}

        ///// <summary>
        ///// 查询指定角色下指定用户
        ///// </summary>
        ///// <param name="roleName">角色名称</param>
        ///// <param name="usernameToMatch">用户名（支持模糊查询）</param>
        ///// <returns></returns>
        //public string[] FindUsersInRole(string roleName, string usernameToMatch, ComponentSearchScope searchScop = ComponentSearchScope.Subtree)
        //{
        //    List<string> users = new List<string>();
        //    switch (searchScop)
        //    {
        //        case ComponentSearchScope.OneLevel:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetOnLevelCAFUsersInRole", roleName, usernameToMatch))
        //            {
        //                while (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    users.Add(reader[0].ToString());
        //                }
        //            }
        //            break;
        //        case ComponentSearchScope.Subtree:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetCAFUsersInRole", roleName, usernameToMatch))
        //            {
        //                while (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    users.Add(reader[0].ToString());
        //                }
        //            }
        //            break;
        //        default:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetCAFUsersInRole", roleName, usernameToMatch))
        //            {
        //                while (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    users.Add(reader.GetString(0));
        //                }
        //            }
        //            break;
        //    }
        //    return users.ToArray();
        //}

        //public string[] GetAllRoles(CustomRoleType roleType = CustomRoleType.All)
        //{
        //    List<string> roles = new List<string>();
        //    using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetALLCAFRoles", (int)roleType))
        //    {
        //        while (reader.Read() && !reader.IsDBNull(0))
        //        {
        //            roles.Add(reader.GetString(0));
        //        }
        //    }
        //    return roles.ToArray();
        //}

        ///// <summary>
        ///// 获取用户所有角色
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //public string[] GetRolesForUser(string username,CustomRoleType roleType=CustomRoleType.All)
        //{
        //    List<string> roles = new List<string>();
        //    using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetCAFRolesForUser", username,(int)roleType))
        //    {
        //        while (reader.Read() && !reader.IsDBNull(0))
        //        {
        //            roles.Add(reader.GetString(0));
        //        }
        //        return roles.ToArray();
        //    }
        //}

        ///// <summary>
        ///// 查询指定角色下所有用户
        ///// </summary>
        ///// <param name="roleName">角色名</param>
        ///// <param name="searchScope">查询范围</param>
        ///// <returns></returns>
        //public string[] GetUsersInRole(string roleName, ComponentSearchScope searchScope = ComponentSearchScope.Subtree)
        //{
        //    return FindUsersInRole(roleName, null, searchScope);
        //}

        //public bool IsUserInRole(string username, string roleName)
        //{
        //    string[] roles = GetRolesForUser(username);
        //    return roles.Contains(roleName);
        //}

        //public void RemoveUserFromRole(string username, string roleName)
        //{
        //    Roles.RemoveUserFromRole(username, roleName);
        //}
        //public void RemoveUserFromRoles(string username, string[] roleNames)
        //{
        //    Roles.RemoveUserFromRoles(username, roleNames);
        //}
        //public void RemoveUsersFromRole(string[] usernames, string roleName)
        //{
        //    Roles.RemoveUsersFromRole(usernames, roleName);
        //}
        //public void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        //{
        //    Roles.RemoveUsersFromRoles(userNames, roleNames);
        //}

        //public bool RoleExists(string roleName)
        //{
        //    return Roles.RoleExists(roleName);
        //}

        ///// <summary>
        ///// 查询指定角色下所有角色
        ///// </summary>
        ///// <param name="roleName">角色名称</param>
        ///// <param name="searchScope">搜索范围</param>
        ///// <returns></returns>
        //public string[] GetRolesInRole(string roleName, ComponentSearchScope searchScope=ComponentSearchScope.Subtree)
        //{
        //    List<string> roles = new List<string>();
        //    switch (searchScope)
        //    {
        //        case ComponentSearchScope.OneLevel:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetOneLevelCAFRolesInRole", roleName))
        //            {
        //                while (reader.Read() &&!reader.IsDBNull(0))
        //                {
        //                    roles.Add(reader.GetString(0));
        //                }
        //            }
        //            break;
        //        case ComponentSearchScope.Subtree:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetCAFRolesInRole", roleName))
        //            {
        //                while (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    roles.Add(reader.GetString(0));
        //                }
        //            }
        //            break;
        //        default:
        //            using (IDataReader reader = db.ExecuteReader("aspnet_Roles_GetCAFRolesInRole", roleName))
        //            {
        //                while (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    roles.Add(reader.GetString(0));
        //                }
        //            }
        //            break;
        //    }
        //    return roles.ToArray();
        //}

        ///// <summary>
        ///// 判断角色是否包含指定角色
        ///// </summary>
        ///// <param name="pRoleName">父角色名</param>
        ///// <param name="cRoleName">子角色名</param>
        ///// <returns></returns>
        //public bool IsRoleInRole(string pRoleName, string cRoleName)
        //{
        //    string[] roles=GetRolesInRole(pRoleName);
        //    return roles.Contains(cRoleName);
        //}

        ///// <summary>
        ///// 创建权限
        ///// </summary>
        ///// <param name="permissionName">权限名称</param>
        //public void GreatePermission(string permissionName)
        //{
        //    db.ExecuteNonQuery("aspnet_Permission_Create", permissionName);
        //}

        //public void AddPermissionToRole(string permissionName, string roleName)
        //{ }
        //public void AddPermissionsToRole(string[] permissionNames,string roleName) { }
        //public void AddPermissionToRoles(string permissionName, string[] roleNames) { }
        //public void AddPermissionsToRoles(string[] permissionNames, string[] roleNames) { }

        //public void DeletePermission(string permissionName)
        //{
        //}
        //public void RemovePermissionFromRole(string permissionName, string roleName) { }
        //public void RemovePermissionsFromRoles(string[] permissionNames, string[] roleName) { }
        //public void RemovePermissionFromRoles(string permissionName, string[] roleNames) { }
        //public void RemovePermissionsFromRole(string permissionName, string[] roleNames) { }

        //public string GetPermissionsInRole(string roleName, ComponentSearchScope searchScope)
        //{
        //    return "";
        //}

        //public string[] GetAllPermissions()
        //{
        //    return new string[2];
        //}

        //public string[] GetPermissionsForUser(string username)
        //{
        //    return new string[3];
        //}
    }

    /// <summary>
    ///  指定使用 Component 对象执行的目录搜索的可能范围。
    /// </summary>
    public enum ComponentSearchScope
    {
        /// <summary>
        /// 搜索基对象的直接子对象，但不搜索基对象。
        /// </summary>
        OneLevel = 0,

        /// <summary>
        /// 搜索整个子树，包括基对象及其所有子对象。
        /// </summary>
        Subtree = 1,
    }

    public enum CustomRoleType
    {
        Role = 0,
        Group = 1,
        All = 2
    }
}