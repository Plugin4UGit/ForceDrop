﻿using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using Logger = Rocket.Core.Logging.Logger;
namespace ForceDrop
{
    public class ForceDrop : RocketPlugin<ForceDropConfiguration>
    {
        private static ForceDrop Instance;

        protected override void Load()
        {
            if (Configuration.Instance.Enabled)
            {
                Instance = this;
                Logger.Log("[Plugin4U] ForceDrop loaded!");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Make sure to visit our site at www.Plugin4U.cf");
                Console.ResetColor();
            }
            else {
                Logger.Log("Configuration.Instance.Enabled == false");
                this.Unload();

            }
        }


        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"drop_message","Successfully dropped {0} inventory!" },
                    {"drop_message_id","Successfully dropped {0} from {1} inventory!"},
                };
            }
        }


        [RocketCommand("ForceDrop", "", "", AllowedCaller.Both)]
        [RocketCommandAlias("Fd")]
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (command.Length == 1)
            {
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
                if (player != null)
                {
                    for (byte page = 0; page < 8; page++)
                    {
                        var count = player.Inventory.getItemCount(page);

                        for (byte index = 0; index < count; index++)
                        {
                            foreach (var number in Configuration.Instance.WhiteListedFromDrop)
                            {
                                if (player.Inventory.getItem(page, index).item.id == number)
                                { }
                                else
                                {
                                    player.Inventory.askDropItem(player.CSteamID, page, player.Inventory.getItem(page, index).x, player.Inventory.getItem(page, index).y);
                                }
                            }
                        }
                    }
                    UnturnedChat.Say(caller, Translate("drop_message", player.DisplayName));    
                }
            }
            if(command.Length == 2)
            {
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
                if (player != null)
                {
                    for (byte page = 0; page < 8; page++)
                    {
                        var count = player.Inventory.getItemCount(page);

                        for (byte index = 0; index < count; index++)
                        {
                            if (player.Inventory.getItem(page, index).item.id == ushort.Parse(command[1].ToString()))
                            {
                                player.Inventory.askDropItem(player.CSteamID, page, player.Inventory.getItem(page, index).x, player.Inventory.getItem(page, index).y);
                            }
                        }
                    }
                    UnturnedChat.Say(caller, Translate("drop_message_id", ushort.Parse(command[1].ToString()), player.DisplayName));
                }
            }
        }
    }
}
