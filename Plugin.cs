using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Utilities;
using ProjectMER.Features.Objects;
using SkibidiToilet.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkibidiToilet
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "OpasniDed";
        public override string Name => "SkibidiToilet";
        public override string Prefix => "SkibidiToilet";
        public static Plugin plugin;

        public override void OnEnabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.Died += PlayerDead;
            Exiled.Events.Handlers.Player.ChangingRole += ChangingRole;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.Died -= PlayerDead;
            Exiled.Events.Handlers.Player.ChangingRole -= ChangingRole;
            plugin = null;
            base.OnDisabled();
        }

        public interface IModule
        {
            void Register();
            void Unregister();
        }

        public void PlayerDead(DiedEventArgs ev)
        {
            Clear(ev.Player);
        }
        public void ChangingRole(ChangingRoleEventArgs ev)
        {
            Clear(ev.Player);
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            toilet.audious.Clear();
            toilet.skibidiToilets.Clear();
        }
        public void OnRoundStarted()
        {
            toilet.audious.Clear();
            toilet.skibidiToilets.Clear();
        }

        private void Clear(Player player)
        {
            if (toilet.skibidiToilets.TryGetValue(player, out SchematicObject old))
            {
                if (toilet.audious.TryGetValue(old, out AudioPlayer audio))
                {
                    toilet.audious.Remove(old);
                    audio.Destroy();
                }
                toilet.skibidiToilets.Remove(player);
                old.Destroy();
            }
        }
    }
}
