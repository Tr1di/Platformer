using System;
using Tridi.AI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tridi
{
    public static class PlatformerExtensions
    {
        public static int ToMilliseconds(this float seconds)
        {
            return (int)(seconds * 1000f);
        }

        public static void PlayOneShot(this AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float tail = 1f)
        {
            if (!clip) return;
            
            var go = new GameObject("Sound")
            {
                transform =
                {
                    position = position
                }
            };

            var audio = go.AddComponent<AudioSource>();
            
            audio.clip = clip;
            audio.volume = volume;
            audio.pitch = pitch;
            
            audio.Play();
            Object.Destroy(go, clip.length + tail);
        }
    }
    
    public static class TaskExtensions
    {
        public static IAction Do(this IAction task, Action action)
        {
            return task.Then.Do(action);
        }

        public static IAction Do(this ICondition task, Action action)
        {
            return task.Also.Do(action);
        }

        public static IAction Do(this ICycling task, Action action)
        {
            return task.EveryFixedUpdate().Do(action);
        }

        public static ITask Task(this IAction task)
        {
            return task.Then.Prepare();
        }

        public static ITask Prepare(this IAction task)
        {
            return task.Then.Prepare();
        }

        public static ITask Prepare(this ICondition task)
        {
            return task.Also.Prepare();
        }

        public static ITask Prepare(this ICycling task)
        {
            return task.EveryFixedUpdate().Prepare();
        }

        public static ICondition While(this ICondition condition, Predicate<ITask> predicate)
        {
            return condition.Also.While(predicate);
        }

        public static ICondition While(this ICycling repeat, Predicate<ITask> predicate)
        {
            return repeat.EveryFixedUpdate().While(predicate);
        }

        public static ICondition Until(this ICondition condition, Predicate<ITask> predicate)
        {
            return condition.Also.Until(predicate);
        }

        public static ICondition Until(this ICycling repeat, Predicate<ITask> predicate)
        {
            return repeat.EveryFixedUpdate().Until(predicate);
        }

        public static IRepeat For(this ICondition condition, float count)
        {
            return condition.Also.For(count);
        }

        public static ICycling For(this ICondition condition)
        {
            return condition.Also.For();
        }

        public static IAction Once(this ICondition condition)
        {
            return condition.Also.Once();
        }
        
        public static IDelay WhitDelay(this ICondition condition, float seconds)
        {
            return condition.Also.WithDelay(seconds);
        }

        public static IDelay WhitDelay(this ICycling repeat, float seconds)
        {
            return repeat.EveryFixedUpdate().WithDelay(seconds);
        }
    }
}
