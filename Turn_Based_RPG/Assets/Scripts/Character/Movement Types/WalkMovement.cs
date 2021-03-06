using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace PsychesBound
{
    [CreateAssetMenu(menuName = menuName + "Walk")]
    public class WalkMovement : MovementType
    {





        protected override bool ExpandSearch(Tile from, Tile to, IBattler unit)
        {
            // Skip if the distance in height between the two tiles is more than the unit can jump
            if ((Mathf.Abs(from.height - to.height) > (int)unit.GetStatValue(SecondaryType.JumpHeight)))
                return false;
            // Skip if the tile is occupied by an enemy
            if (to.content != null)
                return false;
            return base.ExpandSearch(from, to, unit);
        }


        protected async virtual UniTask Turn(IBattler unit, Direction dir)
        {
            //TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
            var t = unit.transform.DORotate(dir.ToEuler(), 0.25f, RotateMode.Fast).SetEase(Ease.InOutQuad);

            // When rotating between North and West, we must make an exception so it looks like the unit
            // rotates the most efficient way (since 0 and 360 are treated the same)
            if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
                t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
            else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
                t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
            unit.Direction = dir;

           

            //while (t != null)
            //    yield return null;


            await UniTask.Delay(TimeSpan.FromSeconds(t.Duration()), DelayType.DeltaTime);

            t.Kill(false);
            //yield return new WaitForSeconds(t.Duration());

        }

        async UniTask Walk(IBattler unit, Tile to)
        {
            Tweener tweener = unit.transform.DOMove(to.center, 0.5f, false).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(tweener.Duration()), DelayType.DeltaTime);

            tweener.Kill(false);
        }
        async UniTask Jump(IBattler unit, Tile to)
        {
            Tweener tweener = unit.transform.DOMove(to.center, 0.5f, false).SetEase(Ease.Linear);
            Tweener t2 = unit.Jumper.DOLocalMove(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.Duration() / 2f, false).SetEase(Ease.InOutQuad);
            t2.SetLoops(1, LoopType.Yoyo);
            await UniTask.Delay(TimeSpan.FromSeconds(tweener.Duration()), DelayType.DeltaTime);
            tweener.Kill(false);
        }


        public async override UniTask Traverse(Tile tile, IBattler unit, BattleField field)
        {
            unit.Place(tile, field);
            // Build a list of way points from the unit's 
            // starting tile to the destination tile
            List<Tile> targets = new List<Tile>();
            while (tile != null)
            {
                targets.Insert(0, tile);
                tile = tile.prev;
            }
            // Move to each way point in succession
            for (int i = 1; i < targets.Count; ++i)
            {
                Tile from = targets[i - 1];
                Tile to = targets[i];
                Direction dir = from.GetDirection(to);
                if (unit.Direction != dir)
                {
                    await Turn(unit, dir);
                }

                if (from.height == to.height)
                {
                    await Walk(unit, to);
                }

                else
                {
                    await (Jump(unit, to));
                }

            }
        }
    }
}
