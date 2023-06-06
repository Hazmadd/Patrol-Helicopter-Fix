using System.Collections.Generic;
using System.Linq;
using Oxide.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Oxide.Plugins
{
    [Info("PatrolHelicopterFix", "Hazmad", "1.0.0")]
    [Description("Makes Patrol Helicopter ignore specific monuments or markers.")]
    class PatrolHelicopterFix : RustPlugin
    {
        private List<string> ignoredMonuments;
        private NavMeshAgent patrolHelicopterAgent;

        protected override void LoadDefaultConfig()
        {
            ignoredMonuments = new List<string> { "x", "*", "o" };
            SaveConfig();
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            ignoredMonuments = Config.Get<List<string>>("IgnoredMonuments");
        }

        private void SaveConfig()
        {
            Config.Set("IgnoredMonuments", ignoredMonuments);
            Config.Save();
        }

        void Init()
        {
            LoadConfig();
        }

        void OnEntitySpawned(BaseNetworkable entity)
        {
            if (entity is PatrolHelicopterAI)
            {
                PatrolHelicopterAI helicopter = entity.GetComponent<PatrolHelicopterAI>();
                if (helicopter != null)
                {
                    patrolHelicopterAgent = helicopter.GetComponent<NavMeshAgent>();
                    CalculateAndRemoveIgnoredMonuments();
                }
                else
                {
                    PrintWarning("Failed to get PatrolHelicopterAI component.");
                }
            }
        }

        private void CalculateAndRemoveIgnoredMonuments()
        {
            NavMeshPath patrolHelicopterPath = new NavMeshPath();
            if (patrolHelicopterAgent.CalculatePath(patrolHelicopterAgent.destination, patrolHelicopterPath))
            {
                RemoveIgnoredMonumentsFromPath(patrolHelicopterPath);
            }
            else
            {
                PrintWarning("Failed to calculate patrol helicopter path!");
            }
        }

        private void RemoveIgnoredMonumentsFromPath(NavMeshPath path)
        {
            if (path == null || path.corners.Length < 2)
                return;

            List<Vector3> newPathCorners = new List<Vector3>();

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 startCorner = path.corners[i];
                Vector3 endCorner = path.corners[i + 1];

                RaycastHit hit;
                if (Physics.Linecast(startCorner, endCorner, out hit, LayerMask.GetMask("Terrain", "World", "Construction")))
                {
                    if (hit.collider.CompareTag("Monument"))
                    {
                        string monumentName = hit.collider.name;
                        if (!ignoredMonuments.Contains(monumentName))
                            newPathCorners.Add(hit.point);
                    }
                    else
                    {
                        newPathCorners.Add(hit.point);
                    }
                }
            }

            path.ClearCorners();
            newPathCorners.ForEach(c => path.corners.Append(c));
        }

        object CanHelicopterTarget(PatrolHelicopterAI helicopterAI)
        {
            if (patrolHelicopterAgent != null && patrolHelicopterAgent.path != null && patrolHelicopterAgent.path.corners.Length >= 2)
            {
                Vector3 targetPosition = patrolHelicopterAgent.transform.position;
                if (Vector3.Distance(targetPosition, patrolHelicopterAgent.path.corners.Last()) < 5f)
                    return null;
            }

            return false;
        }
    }
}
