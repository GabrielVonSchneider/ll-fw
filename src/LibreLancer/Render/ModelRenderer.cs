﻿/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * 
 * The Initial Developer of the Original Code is Callum McGing (mailto:callum.mcging@gmail.com).
 * Portions created by the Initial Developer are Copyright (C) 2013-2016
 * the Initial Developer. All Rights Reserved.
 */
using System;
using LibreLancer.Utf.Cmp;
using LibreLancer.Utf.Mat;
using LibreLancer.GameData;
namespace LibreLancer
{
	public class ModelRenderer
	{
		ICamera camera;
		public Matrix4 World { get; private set; }
		public SystemObject SpaceObject { get; private set; }
		public ModelFile Model { get; private set; }
		public CmpFile Cmp { get; private set; }
		public SphFile Sph { get; private set; }
		public string Nebula;

		public ModelRenderer (ICamera camera, Matrix4 world, SystemObject spaceObject,ResourceManager cache, string nebula)
		{
			Nebula = nebula;
			World = world * Matrix4.CreateTranslation(spaceObject.Position);
			if (spaceObject.Rotation != null)
				World = spaceObject.Rotation.Value * World;
			SpaceObject = spaceObject;
			this.camera = camera;
			IDrawable archetype = spaceObject.Archetype.Drawable;
			if (archetype is ModelFile) {
				Model = archetype as ModelFile;
				if (Model != null && Model.Levels.ContainsKey (0)) {
					Model.Initialize (cache);
				}
			} else if (archetype is CmpFile) {
				Cmp = archetype as CmpFile;
				Cmp.Initialize (cache);
			} else if (archetype is SphFile) {
				Sph = archetype as SphFile;
				Sph.Initialize (cache);
			}
		}

		public void Update(TimeSpan elapsed)
		{
			if (Model != null)
				Model.Update (camera, elapsed);
			else if (Cmp != null)
				Cmp.Update (camera, elapsed);
			else if (Sph != null)
				Sph.Update (camera, elapsed);
		}

		public void Draw(CommandBuffer buffer, Lighting lights, string nebula)
		{
			if (Nebula != null && nebula != Nebula)
				return;
			if (Model != null) {
				if (Model.Levels.ContainsKey (0)) {
					var bsphere = new BoundingSphere(
						VectorMath.Transform(Model.Levels[0].Center, World),
						Model.Levels[0].Radius
					);
					if (camera.Frustum.Intersects (bsphere))
						Model.DrawBuffer (buffer, World, lights);
				}
			} else if (Cmp != null) {
				foreach (ModelFile model in Cmp.Models.Values)
					if (model.Levels.ContainsKey (0)) {
						var bsphere = new BoundingSphere(
							VectorMath.Transform(model.Levels[0].Center, World),
							model.Levels[0].Radius
						);
						if (camera.Frustum.Intersects (bsphere)) {
							Cmp.DrawBuffer (buffer, World, lights);
							break;
						}
					}
			} else if (Sph != null) {
				Sph.DrawBuffer (buffer, World, lights); //Need to cull this
			}
		}
	}
}

