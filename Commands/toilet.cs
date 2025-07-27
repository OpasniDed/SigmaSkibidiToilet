using CommandSystem;
using Exiled.API.Features;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SkibidiToilet.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class toilet : ICommand
    {
        public string Command { get; } = "toilet";
        public string[] Aliases { get; } = {};
        public string Description { get; } = "BECOME PLAYER TO SKIBIDI TOILET!!!\nUse: toilet <player id>";
        public bool SanitizeResponse => false;
        private System.Random random = new System.Random();

        private static readonly string AudioFolder = Path.Combine(Paths.Configs, "Audios");
        public static readonly Dictionary<Player, SchematicObject> skibidiToilets = new();
        public static readonly Dictionary<SchematicObject, AudioPlayer> audious = new();

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player == null)
            {
                response = "Player doesnt exist";
                return false;
            }

            if (args.Count == 0)
            {
                response = "Use: toilet <player id>";
                return false;
            }


            if (!Int32.TryParse(args.At(0), out int ide))
            {
                response = "Enter the player ID";
                return false;
            }

            Player target = Player.Get(id: ide);
            if (target == null)
            {
                response = "Player doesnt exist";
                return false;
            }
            if (skibidiToilets.TryGetValue(target, out SchematicObject old))
            {
                if (audious.TryGetValue(old, out AudioPlayer audio))
                {
                    audious.Remove(old);
                    audio.Destroy();
                }
                skibidiToilets.Remove(target);
                old.Destroy();
                response = "SKIBIDI TOILET REMOVED";
                return true;
            }
            

            SchematicObject schematicObject = ObjectSpawner.SpawnSchematic($"{Plugin.plugin.Config.schematicName}", target.Position, Quaternion.Euler(target.Rotation.eulerAngles));
            if (schematicObject == null)
            {
                response = "Schmatic doesnt exist";
                return false;
            }
            skibidiToilets.Add(target, schematicObject);
            target.Scale = new Vector3(0.4f, 0.4f, 0.4f);
            target.Health = Plugin.plugin.Config.Health;
            schematicObject.transform.parent = target.Transform;
            CreateAndPlayAudio($"{Plugin.plugin.Config.soundName}", $"Skibidi_{random.Next(0, 99999)}", true, schematicObject.Position, false, schematicObject.transform.parent, true, 50f, 5f, schematicObject);
            



            response = "SKIBIDI TOILET CREATED!!! WRITE COMMAND AGAIN FOR REMOVE IT";
            return true;
        }

        private static void CreateAndPlayAudio(string FileName, string audioPlayerName, bool loop, Vector3 position, bool detgroyOnEnd = false, Transform parent = null, bool isSpatial = false, float maxDistance = 5, float minDistance = 5, SchematicObject schematicObject = null)
        {
            var audioPlayer = AudioPlayer.CreateOrGet(audioPlayerName);
            var fullPath = Path.Combine(AudioFolder, FileName);
            if (!audioPlayer.TryGetSpeaker(audioPlayerName, out Speaker speaker))
            {
                speaker = audioPlayer.AddSpeaker(audioPlayerName, isSpatial: isSpatial, maxDistance: maxDistance, minDistance: minDistance, volume: 100);
            }
            if (parent)
            {
                speaker.transform.SetParent(parent);
                speaker.transform.localPosition = Vector3.zero;
                speaker.transform.localRotation = Quaternion.identity;
            }
            else
                speaker.Position = position;
            audious.Add(schematicObject, audioPlayer);
            if (!AudioClipStorage.AudioClips.ContainsKey(FileName))
                AudioClipStorage.LoadClip(fullPath, FileName);
            audioPlayer.AddClip(FileName, destroyOnEnd: detgroyOnEnd, loop: loop);
        }
    }
}
