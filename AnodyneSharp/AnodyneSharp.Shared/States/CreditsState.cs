﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnodyneSharp.Dialogue;
using AnodyneSharp.Drawing;
using AnodyneSharp.Entities;
using AnodyneSharp.Input;
using AnodyneSharp.Registry;
using AnodyneSharp.Sounds;
using AnodyneSharp.UI;
using AnodyneSharp.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnodyneSharp.States
{
    public class CreditsState : State
    {
        private List<UILabel> _labels;
        private List<UIEntity> _entities;

        private UIEntity _dimOverlay;
        private UIEntity _bg;
        private Screenie _screenie;

        private IEnumerator _stateLogic;
        private bool _stopScroll;

        public CreditsState()
        {
            _labels = new List<UILabel>();
            _entities = new List<UIEntity>();

            int bgHeight = 480;

            _dimOverlay = new UIEntity(Vector2.Zero, "dim_overlay", 160, 180, DrawOrder.PAUSE_SELECTOR)
            {
                opacity = 0.2f
            };

            _bg = new UIEntity(new Vector2(0, -bgHeight + 180), "go", 160, bgHeight, DrawOrder.FOOT_OVERLAY);

            _screenie = new Screenie();

            int y = 0;

            for (int i = 0; i < 24; i++)
            {
                if (i != 0 && i % 2 == 0)
                {
                    y += 195;
                }
                else
                {
                    y += 180;
                }


                string text = DialogueManager.GetDialogue("misc", "any", "ending", i);

                _labels.Add(new UILabel(new Vector2(0, y), true, text, layer: DrawOrder.TEXT, centerText: true));
            }

            CreateEntities();

            _stateLogic = StateLogic();
        }

        public override void Update()
        {
            base.Update();

            _stateLogic.MoveNext();

            _screenie.Update();

            foreach (var entity in _entities)
            {
                entity.PostUpdate();
            }

            if (!_stopScroll)
            {
                if (KeyInput.IsRebindableKeyPressed(KeyFunctions.Accept))
                {
                    if (GlobalState.FUCK_IT_MODE_ON)
                    {
                        GameTimes.TimeScale = 80;
                    }
                    else
                    {
                        GameTimes.TimeScale = 8;
                    }
                }
                else
                {
                    GameTimes.TimeScale = 1;
                }

                float speed = 15 * GameTimes.DeltaTime;

                foreach (var label in _labels)
                {
                    label.Position += new Vector2(0, -speed);
                }
            }
            else
            {
                GameTimes.TimeScale = 1;
            }
        }

        public override void DrawUI()
        {
            base.DrawUI();

            _dimOverlay.Draw();
            _bg.Draw();

            _screenie.Draw();

            foreach (var label in _labels)
            {
                label.Draw();
            }

            foreach (var entity in _entities)
            {
                entity.Draw();
            }
        }

        private IEnumerator StateLogic()
        {
            SoundManager.PlaySong("ending");

            while (!MathUtilities.MoveTo(ref _bg.Position.Y, 0, 15))
            {
                yield return null;
            }

            int index = 6;

            while (index < 24)
            {
                ProgressScreenie(ref index);
                yield return null;
            }

            while (!MathUtilities.MoveTo(ref _dimOverlay.opacity, 0, 0.06f))
            {
                yield return null;
            }

            _labels.Last().Position = new Vector2(0, 0);
            _stopScroll = true;
            yield break;
        }

        private void ProgressScreenie(ref int index)
        {
            while (_labels[index].Position.Y > 180)
            {
                return;
            }

            index++;

            _screenie.ProgressFrame();
        }

        private void CreateEntities()
        {
            int i = 12;

            Vector2 lPos = _labels[i++].Position;

            CreateEntity(lPos + new Vector2(4, 26) + new Vector2(4, 10), "slime", new Point(16), 5, false, 0, 1);
            CreateEntity(lPos + new Vector2(130, 46) + new Vector2(4, 20), "annoyer", new Point(16), 8, false, 0, 1, 2, 3, 4, 5);
            CreateEntity(lPos + new Vector2(5, 66) + new Vector2(4, 30), "pew_laser", new Point(16), 0, false, 0);

            CreateEntity(lPos + new Vector2(130, 92) + new Vector2(4, 30), "shieldy", new Point(16), 5, false, 1, 2, 1, 0, 1, 2, 1, 0, 16, 17, 18);
            CreateEntity(lPos + new Vector2(5, 84) + new Vector2(4, 30), "pew_laser_bullet", new Point(16, 8), 8, false, 0, 1);

            CreateEntity(lPos + new Vector2(11, 110) + new Vector2(4, 30), "sun_guy", new Point(16, 24), 3, false, 0, 1, 2, 3, 4);
            CreateEntity(lPos + new Vector2(125, 120) + new Vector2(4, 30), "light_orb", new Point(16), 6, false, 0, 1, 2, 3, 4, 3, 2, 1);
            CreateEntity(lPos + new Vector2(8, 144) + new Vector2(4, 30), "sun_guy_wave", new Point(128, 8), 8, false, 3, 4, 5);

            lPos = _labels[i++].Position;

            CreateEntity(lPos + new Vector2(35, -4) + new Vector2(4, 10), "f_mover", new Point(16), 4, false, 0, 1);
            CreateEntity(lPos + new Vector2(108, 30) + new Vector2(4, 10), "on_off_shooter", new Point(16), 2, false, 0, 1, 2, 2, 1, 0);
            CreateEntity(lPos + new Vector2(4, 20) + new Vector2(4, 10), "f_four_shooter", new Point(16), 3, false, 0, 1, 2, 2, 1, 0);
            CreateEntity(lPos + new Vector2(115, 68) + new Vector2(4, 10), "f_slasher", new Point(24), 3, false, 0, 1, 0, 1, 0, 1);
            CreateEntity(lPos + new Vector2(12, 110) + new Vector2(4, 2), "red_boss", new Point(32), 3, false, 0, 0, 1, 0, 0, 2);
            CreateEntity(lPos + new Vector2(4, 138) + new Vector2(4, 2), "red_boss_ripple", new Point(48, 8), 12, false, 0, 1);

            lPos = _labels[i++].Position;

            CreateEntity(lPos + new Vector2(22, -3) + new Vector2(4, 10), "dog", new Point(16), 4, false, 2, 3, 2, 3, 4, 5, 4, 5, 6, 7, 6, 7, 2, 3);
            CreateEntity(lPos + new Vector2(118, 20) + new Vector2(4, 10), "frog", new Point(16), 2, false, 0, 1, 0, 1, 3, 3);

            CreateEntity(lPos + new Vector2(20, 42) + new Vector2(4, 10), "f_rotator", new Point(16), 10, false, 0, 1);
            CreateEntity(lPos + new Vector2(120, 68) + new Vector2(4, 10), "person", new Point(16), 5, false, 0, 1, 0, 1, 2, 3, 2, 3, 4, 5, 4, 5, 2, 3, 2, 3);

            CreateEntity(lPos + new Vector2(47, 120) + new Vector2(4, 10), "f_wallboss_face", new Point(64, 32), 3, false, 0, 0, 1, 0, 0, 2);
            CreateEntity(lPos + new Vector2(-1, 120) + new Vector2(4, 10), "wallboss_wall", new Point(160, 32), 4, false, 0, 1, 0, 1, 0, 1);

            CreateEntity(lPos + new Vector2(8, 150) + new Vector2(4, 10), "f_wallboss_l_hand", new Point(32), 1, false, 0, 1, 2, 3);
            CreateEntity(lPos + new Vector2(118, 150) + new Vector2(4, 10), "f_wallboss_l_hand", new Point(32), 1, true, 0, 1, 2, 3);

            lPos = _labels[i++].Position;

            CreateEntity(lPos + new Vector2(16, -2) + new Vector2(16, 0), "rat", new Point(16), 5, false, 0, 1);

            CreateEntity(lPos + new Vector2(122, 20) + new Vector2(4, 10), "gas_guy", new Point(16, 24), 4, false, 0, 1);
            CreateEntity(lPos + new Vector2(137, 34) + new Vector2(4, 10), "gas_guy_cloud", new Point(24), 3, false, 0, 1);

            CreateEntity(lPos + new Vector2(5, 46) + new Vector2(4, 10), "silverfish", new Point(16), 5, false, 4, 5);
            CreateEntity(lPos + new Vector2(137, 66) + new Vector2(4, 10), "dash_trap", new Point(16), 12, false, 4, 5);

            CreateEntity(lPos + new Vector2(5, 78) + new Vector2(4, 10), "spike_roller_horizontal", new Point(128, 16), 5, false, 0, 1);
            CreateEntity(lPos + new Vector2(70, 120) + new Vector2(4, 30), "splitboss", new Point(24, 32), 5, false, 0, 1, 2, 1);
        }

        private void CreateEntity(Vector2 pos, string texture, Point size, int framerate, bool flipped, params int[] frames)
        {
            UIEntity e = new UIEntity(pos, texture, size.X, size.Y, DrawOrder.TEXT, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            e.AddAnimation("a", frames, framerate);
            e.Play("a");

            e.velocity.Y = -15;

            _entities.Add(e);
        }

        private class Screenie
        {
            UIEntity _overlay;

            UIEntity _entity1;
            UIEntity _entity2;

            int _frame;
            bool _done;

            public Screenie()
                : base()
            {
                _overlay = new UIEntity(Vector2.Zero, "dim_overlay", 160, 180, DrawOrder.HITBOX)
                {
                    opacity = 0f
                };

                _entity1 = new UIEntity(new Vector2(0, 10), "screenies", 160, 160, DrawOrder.HEADER)
                {
                    opacity = 0f
                };

                _entity2 = new UIEntity(new Vector2(0, 10), "screenies", 160, 160, DrawOrder.TEXTBOX)
                {
                    opacity = 0f
                };

                _frame = -1;
                _done = false;
            }

            public void Update()
            {
                if (_done)
                {
                    return;
                }

                if (_frame == 0)
                {
                    if (MathUtilities.MoveTo(ref _entity1.opacity, 1, 0.54f) ||
                        MathUtilities.MoveTo(ref _overlay.opacity, 1, 0.54f))
                    {
                        _entity1.opacity = 1;
                        _overlay.opacity = 1;

                        _done = true;
                    }
                }
                else
                {
                    if (MathUtilities.MoveTo(ref _entity2.opacity, 0, 0.54f))
                    {
                        _done = true;
                    }
                }
            }

            public void Draw()
            {
                _overlay.Draw();
                _entity1.Draw();
                _entity2.Draw();
            }

            public int ProgressFrame()
            {
                _frame++;

                _done = false;

                if (_frame == 0)
                {
                    _entity1.SetFrame(_frame);

                    _entity1.opacity = 0f;
                }
                else
                {
                    _entity1.SetFrame(_frame);
                    _entity2.SetFrame(_frame - 1);

                    _entity1.opacity = 1f;
                    _entity2.opacity = 1f;
                }

                return _frame;
            }
        }
    }
}