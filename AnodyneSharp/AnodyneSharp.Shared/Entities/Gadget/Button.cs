﻿using AnodyneSharp.Drawing;
using AnodyneSharp.Registry;
using AnodyneSharp.Sounds;

namespace AnodyneSharp.Entities.Gadget
{
    //TODO: Add other entity types buttons collide with
    [NamedEntity, Collision(typeof(Player))]
    class Button : Entity
    {
        bool pressed = false;
        bool incremented = false;
        bool permanent;

        public Button(EntityPreset preset, Player p) : base(preset.Position, "buttons", 16, 16, DrawOrder.BG_ENTITIES)
        {
            permanent = preset.Frame == 1;
            switch(GlobalState.CURRENT_MAP_NAME)
            {
                case "STREET":
                    SetFrame(6);
                    break;
                case "BEDROOM":
                    SetFrame(0);
                    break;
                case "REDCAVE":
                    SetFrame(4);
                    break;
                case "CELL":
                    SetFrame(8);
                    break;
                default:
                    SetFrame(2);
                    break;
            }
        }

        public override void Collided(Entity other)
        {
            if(other is Player p)
            {
                pressed |= p.state == PlayerState.GROUND;
            }
            else
            {
                pressed = true;
            }
        }

        public override void Update()
        {
            if(pressed && !incremented)
            {
                incremented = true;
                GlobalState.PUZZLES_SOLVED++;
                SetFrame(_curFrame + 1);
                SoundManager.PlaySoundEffect("button_down");
            }
            else if(!pressed && incremented && !permanent)
            {
                GlobalState.PUZZLES_SOLVED--;
                incremented = false;
                SetFrame(_curFrame - 1);
                SoundManager.PlaySoundEffect("button_up");
            }

            pressed = false;
        }
    }
}
