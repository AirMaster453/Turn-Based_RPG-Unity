using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
using Lowscope.Saving;


namespace PsychesBound
{

    public delegate void OnRoleChange(Role oldRole, Role newRole);
    public class RoleManager : MonoBehaviour
    {
        /// <summary>
        /// main role of the class. level reqs and stat bonuses are determined by this class
        /// </summary>
        [SerializeField, Min(-1)]
        private int mainRole = -1;

        [SerializeField, Min(-1)]
        private int subRole = -1;

        [SerializeField]
        private List<Role> roles = new List<Role>();


        private List<Ability> abilities = new List<Ability>();

        public ReadOnlyCollection<Ability> Abilities => abilities.AsReadOnly();

        private StatManager stats;

        /// <summary>
        /// Event to be called when the main role is changed
        /// </summary>
        public event OnRoleChange OnMainRoleChange;

        /// <summary>
        /// event to be called when sub role is changed
        /// </summary>
        public event OnRoleChange OnSubRoleChange;


        public Role MainRole
        {
            get
            {
                return HasMainRole() ? roles[mainRole] : null;
            }
        }

        public Role SubRole
        {
            get
            {
                return HasSubRole() ? roles[subRole] : null;
            }
        }

        public bool AddRole(RoleType role)
        {
            if (roles.Find((Role r) => r.RoleType == role) != null)
            {
                var r = new Role(role);

                roles.Add(r);
                return true;
            }

            return false;
        }

        public bool HasMainRole()
        {
            return mainRole != -1 && mainRole < roles.Count - 1;
        }

        public bool HasSubRole()
        {
            return subRole != -1 && subRole < roles.Count - 1;
        }

        public void RemoveMainRole()
        {
            mainRole = -1;
        }

        public void RemoveSubRole()
        {
            subRole = -1;
        }

        public void GainExperience(long exp)
        {
            MainRole?.Level?.GainExperienceAsync(exp);

            SubRole?.Level?.GainExperienceAsync(exp);
        }

        //public bool ChangeBothRoles(int main, int sub)
        //{
        //    try
        //    {
        //        if (roles[main] != null && roles[sub] != null)
        //        {
        //            if (index == subRole)
        //            {
        //                subRole = mainRole;
        //            }
        //            mainRole = index;

        //            abilities.Clear();

        //            abilities.AddRange(roles[mainRole].RoleType.Abilities);

        //            if (HasSubRole())
        //            {
        //                var a = new List<Ability>(roles[subRole].RoleType.Abilities).FindAll((Ability a) => !abilities.Contains(a));
        //                abilities.AddRange(a);
        //            }

        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //        return false;
        //    }
        //}

        public bool ChangeMainRole(int index, bool updateAbilities = true)
        {
            try
            {
                if(roles[index] != null && index != mainRole)
                {
                    if(index == subRole)
                    {
                        subRole = mainRole;

                        OnSubRoleChange?.Invoke(roles[mainRole], roles[subRole]);
                    }

                    OnMainRoleChange?.Invoke(roles[mainRole], roles[index]);
                    mainRole = index;

                    if (updateAbilities)
                    {
                        abilities.Clear();

                        abilities.AddRange(roles[mainRole].RoleType.Abilities);

                        if (HasSubRole())
                        {
                            var a = new List<Ability>(roles[subRole].RoleType.Abilities).FindAll((Ability a) => !abilities.Contains(a));
                            abilities.AddRange(a);
                        }
                    }

                    return true;
                }

                return false;
            }
            catch(IndexOutOfRangeException)
            {
                return false;
            }
        }


        public bool ChangeSubRole(int index, bool updateAbilities = true)
        {
            try
            {
                if (roles[index] != null && index != subRole)
                {
                    if (index == mainRole)
                    {
                        mainRole = subRole;
                        OnMainRoleChange?.Invoke(roles[subRole], roles[mainRole]);
                    }
                    OnSubRoleChange?.Invoke(roles[subRole], roles[index]);
                    subRole = index;

                    if (updateAbilities)
                    {
                        abilities.Clear();

                        abilities.AddRange(roles[mainRole].RoleType.Abilities);

                        if (HasSubRole())
                        {
                            var a = new List<Ability>(roles[subRole].RoleType.Abilities).FindAll((Ability a) => !abilities.Contains(a));
                            abilities.AddRange(a);
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        private void Start()
        {
            stats = GetComponent<StatManager>();
            stats.RemoveFromSource(this);
            if (HasMainRole())
            {
                roles[mainRole].InitialBaseValues(stats.Tree);
                stats.Equip(MainRole);
            }

            if(HasSubRole())
                roles[subRole].InitialBaseValues(stats.Tree);

            
        }
    }
}