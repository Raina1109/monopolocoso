// Project: XnaShooter, File: Mission.cs
// Namespace: XnaShooter.GameScreens, Class: Mission
// Path: C:\code\XnaShooter\GameScreens, Author: Abi
// Code lines: 46, Size of file: 857 Bytes
// Creation date: 01.11.2005 23:55
// Last modified: 08.12.2005 16:14
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaShooter.Graphics;
using XnaShooter.Helpers;
using XnaShooter.Game;
using XnaShooter.Sounds;
using Texture = XnaShooter.Graphics.Texture;
using Model = XnaShooter.Graphics.Model;
using XnaShooter.Shaders;
#endregion

namespace XnaShooter.GameScreens
{
	/// <summary>
	/// Mission, just manages the on screen display for the game.
	/// Controlling is done in SpaceCamera class.
	/// Most graphical stuff is done in AsteroidManager.
	/// </summary>
	public class Mission : IGameScreen
	{
		#region Variables
		/// <summary>
		/// Link to game.hudTexture and inGameTexture, the texture itself is not
		/// loaded here, but in the main game. This are just references.
		/// </summary>
		private Texture hudTopTexture, hudBottomTexture;

		/// <summary>
		/// Background model for the landscape ground and all objects
		/// we render on it.
		/// </summary>
		internal Model[] landscapeModels = null,
			shipModels = null,
			itemModels = null;

		/// <summary>
		/// Landscape model size for rendering.
		/// </summary>
		float[] LandscapeModelSize = new float[]
			{
				// LandscapeGround,
				100,
				// Building1,
				12,//25,
				// Building2,
				11,//35,
				// Building3,
				11,//20,
				// Building4,
				11,//25,
				// Building5,
				12,//30,
				// Kaktus1,
				3.5f,//4,
				// Kaktus2,
				3.5f,//3,
				// Kaktus3,
				4.0f,//4,
				// Kaktus4,
				4.5f,//5,
			};
		/// <summary>
		/// Number of landscape models
		/// </summary>
		const int NumOfLandscapeModels = 10;

		/// <summary>
		/// Model size for all items.
		/// </summary>
		internal const float ItemModelSize = 2.5f;

		/// <summary>
		/// Landscape model size for rendering.
		/// </summary>
		internal static readonly float[] ShipModelSize = new float[]
			{
				// OwnShip
				5,
				// Corvette
				5,
				// SmallTransporter
				4,
				// Firebird
				7,
				// RocketFrigate
				13,
				// Rocket
				1.25f,
				// Asteroid
				3.4f,
			};
		/// <summary>
		/// All ships z height
		/// </summary>
		public const float AllShipsZHeight = 5;

		/// <summary>
		/// Ship model types
		/// </summary>
		internal enum ShipModelTypes
		{
			OwnShip,
			Corvette,
			SmallTransporter,
			Firebird,
			RocketFrigate,
			Rocket,
			Asteroid,
		} // enum ShipModelTypes

		/// <summary>
		/// Matrix and number for landscape objects
		/// </summary>
		class MatrixAndNumber
		{
			public Matrix renderMatrix;
			public int number;

			public MatrixAndNumber(Matrix setMatrix, int setNumber)
			{
				renderMatrix = setMatrix;
				number = setNumber;
			} // MatrixAndNumber(setMatrix, setNumber)
		} // class MatrixAndNumber

		/// <summary>
		/// Only keep lists of this and the next segment landscape objects.
		/// </summary>
		List<MatrixAndNumber>
			thisLandscapeSegmentObjects = new List<MatrixAndNumber>(),
			nextLandscapeSegmentObjects = new List<MatrixAndNumber>();
		
		/// <summary>
		/// Generated landscape segment number for objects.
		/// </summary>
		int generatedLandscapeSegmentNumber = 0;

		/// <summary>
		/// Max. number of objects we generate each segment
		/// </summary>
		const int MaxNumberOfObjectsGeneratedEachSegment = 24;

		/// <summary>
		/// Segment length
		/// </summary>
		public const float SegmentLength = 122,// 122.12f,
			TotalSegments = 40,
			ViewDistance = 70,
			GroundZValue = -16,
			LookAheadYValue = 40;

		/// <summary>
		/// Look at position.
		/// </summary>
		static Vector3 lookAtPosition = new Vector3(0, 0, -16);

		/// <summary>
		/// Look at position.
		/// </summary>
		public static Vector3 LookAtPosition
		{
			get
			{
				return lookAtPosition;
			} // get
		} // LookAtPosition
		#endregion

		#region Units
		/// <summary>
		/// Current enemy units.
		/// </summary>
		internal static List<Unit> units = new List<Unit>();

		/// <summary>
		/// Add unit
		/// </summary>
		/// <param name="unitType">Unit</param>
		/// <param name="position">Pos</param>
		public static void AddEnemyUnit(
			Unit.UnitTypes unitType, Vector2 position,
			Unit.MovementPattern movementAI)
		{
			units.Add(new Unit(unitType, position, movementAI));
		} // AddWeaponProjectile(weaponType, position, direction)

		/// <summary>
		/// Render enemy units
		/// </summary>
		private void RenderEnemyUnits()
		{
			for (int num = 0; num < units.Count; num++)
			{
				// Remove this enemy unit if it died
				if (units[num].Render(this))
				{
					// If this is a transporter or rocket frigate,
					// create an item here in 25% of the time we kill such a unit.
					if ((units[num].unitType == Unit.UnitTypes.SmallTransporter ||
						units[num].unitType == Unit.UnitTypes.RocketFrigate) &&
						RandomHelper.GetRandomInt(
						// Create more items if we only have the MG
						Player.currentWeapon == Player.WeaponTypes.MG ? 2 : 3) == 0)
						AddItem((Item.ItemTypes)RandomHelper.GetRandomInt(6),
							units[num].position);

					units.RemoveAt(num);
					num--;
				} // if
			} // for
		} // RenderEnemyUnits()
		#endregion

		#region Items
		/// <summary>
		/// Items.
		/// </summary>
		internal static List<Item> items = new List<Item>();

		/// <summary>
		/// Add item
		/// </summary>
		/// <param name="itemType">Item</param>
		/// <param name="position">Pos</param>
		public static void AddItem(
			Item.ItemTypes itemType, Vector2 position)
		{
			// Don't create a item we currently have as our weapon
			if (itemType == Item.ItemTypes.Mg &&
				Player.currentWeapon == Player.WeaponTypes.MG ||
				itemType == Item.ItemTypes.Gattling &&
				Player.currentWeapon == Player.WeaponTypes.Gattling ||
				itemType == Item.ItemTypes.Plasma &&
				Player.currentWeapon == Player.WeaponTypes.Plasma ||
				itemType == Item.ItemTypes.Rockets &&
				Player.currentWeapon == Player.WeaponTypes.Rockets)
			{
				if (Player.health < 0.75f)
					itemType = Item.ItemTypes.Health;
				else if (Player.empBombs < 5)
					itemType = Item.ItemTypes.Emp;
				else
					itemType = (Item.ItemTypes)(((int)itemType + 1) % 6);
			} // if
			// Always give health item if health is low.
			if (Player.health < 0.25f)
				itemType = Item.ItemTypes.Health;

			items.Add(new Item(itemType, position));
		} // AddItem(itemType, position)

		/// <summary>
		/// Render items
		/// </summary>
		private void RenderItems()
		{
			for (int num = 0; num < items.Count; num++)
			{
				// Remove items if we are done
				if (items[num].Render(this))
				{
					items.RemoveAt(num);
					num--;
				} // if
			} // for
		} // RenderItems()
		#endregion

		#region Weapon projectiles
		/// <summary>
		/// Current weapon projectiles.
		/// </summary>
		static List<Projectile> weaponProjectiles = new List<Projectile>();

		/// <summary>
		/// Add weapon projectile
		/// </summary>
		/// <param name="weaponType">Weapon</param>
		/// <param name="position">Pos</param>
		/// <param name="ownProjectile">True for player, false for enemy</param>
		public static void AddWeaponProjectile(
			Projectile.WeaponTypes weaponType, Vector3 position,
			bool ownProjectile)
		{
			weaponProjectiles.Add(
				new Projectile(weaponType, position, ownProjectile));
		} // AddWeaponProjectile(weaponType, position, direction)

		/// <summary>
		/// Render weapon projectiles
		/// </summary>
		private void RenderWeaponProjectiles()
		{
			for (int num = 0; num < weaponProjectiles.Count; num++)
			{
				// Remove weapon projectile if we are done
				if (weaponProjectiles[num].Render(this))
				{
					weaponProjectiles.RemoveAt(num);
					num--;
				} // if
			} // for
		} // RenderWeaponProjectiles()
		#endregion

		#region Properties
		/// <summary>
		/// Name of this game screen
		/// </summary>
		/// <returns>String</returns>
		public string Name
		{
			get
			{
				return "Mission";
			} // get
		} // Name

		private bool quit = false;
		/// <summary>
		/// Returns true if we want to quit this screen and return to the
		/// previous screen. If no more screens are left the game is exited.
		/// </summary>
		/// <returns>Bool</returns>
		public bool Quit
		{
			get
			{
				return quit;
			} // get
		} // Quit
		#endregion

		#region Constructor
		/// <summary>
		/// Create mission
		/// </summary>
		public Mission()
		{
			Player.Reset();
			thisLandscapeSegmentObjects = GenerateLandscapeSegment(0);
			nextLandscapeSegmentObjects = GenerateLandscapeSegment(1);
		} // Mission()
		#endregion

		#region Generate landscape segment
		/// <summary>
		/// Generate landscape segment
		/// </summary>
		/// <param name="segmentNumber">Segment number</param>
		private List<MatrixAndNumber> GenerateLandscapeSegment(int segmentNumber)
		{
			#region Generate enemy units
			int numOfNewEnemies = 10 + RandomHelper.GetRandomInt(segmentNumber / 8);
			if (segmentNumber == 0)
				numOfNewEnemies = 0;
			// Finished level?
			if (segmentNumber > TotalSegments)
			{
				Player.victory = true;
				Sound.PlayVictorySound();
				Player.SetGameOverAndUploadHighscore();
			} // if
			else
			{
				for (int num = 0; num < numOfNewEnemies; num++)
				{
					AddEnemyUnit((Unit.UnitTypes)
						(segmentNumber < 3 ? RandomHelper.GetRandomInt(2) :
						segmentNumber < 7 ? RandomHelper.GetRandomInt(3) :
						segmentNumber < 11 ? RandomHelper.GetRandomInt(4) :
						segmentNumber < 24 ? RandomHelper.GetRandomInt(5) :
						segmentNumber < 36 ? 1 + RandomHelper.GetRandomInt(4) :
						2 + RandomHelper.GetRandomInt(3)),
						new Vector2(RandomHelper.GetRandomFloat(-20, +20),
						segmentNumber * SegmentLength +
						RandomHelper.GetRandomFloat(-SegmentLength / 2, 3*SegmentLength / 2)),
						(Unit.MovementPattern)
						RandomHelper.GetRandomInt(Unit.NumOfMovementPatterns));
				} // for
			} // else
			#endregion

			#region Generate random landscape objects on the ground
			generatedLandscapeSegmentNumber = segmentNumber;
			List<MatrixAndNumber> ret = new List<MatrixAndNumber>();
			int numOfNewObjects = RandomHelper.GetRandomInt(
				MaxNumberOfObjectsGeneratedEachSegment);
			if (numOfNewObjects < 8)
				numOfNewObjects = 8;

			for (int num = 0; num < numOfNewObjects; num++)
			{
				int type = 1+RandomHelper.GetRandomInt(NumOfLandscapeModels-1);
				// Create buildings only left and right
				if (type <= 5)
				{
					int rotSimple = RandomHelper.GetRandomInt(4);
					float rot = rotSimple == 0 ? 0 :
						rotSimple == 1 ? MathHelper.PiOver2 :
						rotSimple == 1 ? MathHelper.Pi : MathHelper.PiOver2 * 3;
					bool side = RandomHelper.GetRandomInt(2) == 0;
					float yPos = segmentNumber * SegmentLength + 0.94f *
						RandomHelper.GetRandomFloat(-SegmentLength / 2, SegmentLength / 2);
					Vector3 pos = new Vector3(side ? -18 : +18, yPos, -16);
					// Do we have this position already?
					bool tooClose = false;
					int tries = 10;
					do
					{
						tooClose = false;
						foreach (MatrixAndNumber obj in ret)
							if (Vector3.Distance(obj.renderMatrix.Translation, pos) < 16)
								tooClose = true;
						if (tooClose)
						{
							yPos = segmentNumber * SegmentLength + 0.92f *
								RandomHelper.GetRandomFloat(-SegmentLength / 2, SegmentLength / 2);
							side = RandomHelper.GetRandomInt(2) == 0;
							pos = new Vector3(side ? -18 : +18, yPos, -15);
						} // if
					} while (tooClose && tries-- > 0);

					// Add very little height to each object to avoid same height
					// if buildings collide into each other.
					pos += new Vector3(0, 0, 0.001f * num);
					ret.Add(new MatrixAndNumber(
						Matrix.CreateScale(LandscapeModelSize[type]) *
						Matrix.CreateRotationZ(rot) *
						Matrix.CreateTranslation(pos),
						type));
				} // if
				else
				{
					ret.Add(new MatrixAndNumber(
						Matrix.CreateScale(LandscapeModelSize[type]) *
						Matrix.CreateRotationZ(
						RandomHelper.GetRandomFloat(0, MathHelper.Pi * 2)) *
						Matrix.CreateTranslation(new Vector3(
						RandomHelper.GetRandomFloat(-20, +20),
						segmentNumber * SegmentLength +
						RandomHelper.GetRandomFloat(-SegmentLength / 2, SegmentLength / 2),
						-15)),
						type));
				} // else
			} // for
			#endregion

			return ret;
		} // GenerateLandscapeSegment(segmentNumber)
		#endregion

		#region Render level background
		/// <summary>
		/// Helper structure to keep models and matrices for rendering in a list.
		/// </summary>
		internal struct ModelAndMatrix
		{
			public Model model;
			public Matrix matrix;
		} // struct ModelAndMatrix

		/// <summary>
		/// Max. number of models we can render each frame. We got between 20 and
		/// 50 landscape objects (ground+objects), and usually between 5 and 20
		/// ships on the screen. This list should be sufficiant for most scenes,
		/// if we got more than 100 models a few landscape objects might be
		/// skipped (they are rendered at last).
		/// </summary>
		const int MaxNumberOfModelsToRender = 100;

		/// <summary>
		/// List of all models we have to render this frame. The main reason
		/// for this list is maintainability because we have to render everything
		/// 3 times: Once for shadow map generation, once for receiving shadows
		/// and finally on the screen.
		/// The list is created only once and is reused, dynamic lists and foreach
		/// would be too slow on the Xbox 360.
		/// </summary>
		internal ModelAndMatrix[] modelsToRender =
			new ModelAndMatrix[MaxNumberOfModelsToRender];

		/// <summary>
		/// Keep track on how many models we currently have in our list. If this
		/// reaches MaxNumberOfModelsToRender no more new objects are added.
		/// </summary>
		internal int numOfModelsToRender = 0;

		/// <summary>
		/// Helper method to simply add a new model for rendering.
		/// </summary>
		/// <param name="setModel">Model</param>
		/// <param name="renderMatrix">Render Matrix</param>
		internal void AddModelToRender(Model setModel, Matrix renderMatrix)
		{
			if (numOfModelsToRender >= MaxNumberOfModelsToRender)
				return;

			modelsToRender[numOfModelsToRender].model = setModel;
			modelsToRender[numOfModelsToRender].matrix = renderMatrix;
			numOfModelsToRender++;
		} // AddModelToRender(setModel, renderMatrix)

		/// <summary>
		/// Render level background
		/// </summary>
		public void RenderLevelBackground(float levelPosition)
		{
#if DEBUG
			// Landscape or unit ship models not initialized yet?
			// Can happen in certain unit tests, initialize them for us!
			if (landscapeModels == null)
				landscapeModels = new Model[]
					{
						new Model("BackgroundGround"),
						new Model("Building"),
						new Model("Building2"),
						new Model("Building3"),
						new Model("Building4"),
						new Model("Building5"),
						new Model("Kaktus"),
						new Model("Kaktus2"),
						new Model("KaktusBenny"),
						new Model("KaktusSeg"),
					};
			if (shipModels == null)
				shipModels = new Model[]
					{
						new Model("OwnShip"),
						new Model("Corvette"),
						new Model("SmallTransporter"),
						new Model("Firebird"),
						new Model("RocketFrigate"),
						new Model("Rocket"),
						new Model("Asteroid"),
					};
			if (itemModels == null)
				itemModels = new Model[]
					{
						new Model("ItemMg"),
						new Model("ItemGattling"),
						new Model("ItemPlasma"),
						new Model("ItemRockets"),
						new Model("ItemEmp"),
					};
#endif

			#region Update camera
			// Construct camera position, it will just move up.
			Vector3 cameraPosition = new Vector3(0, levelPosition, ViewDistance);
			// For widescreen look closer to the ground, else it does not fit on the screen
			if ((float)BaseGame.Width / (float)BaseGame.Height >= 1.5f)
			{
				cameraPosition.Z -= 10;
				cameraPosition.Y -= 2;
			} // if
			lookAtPosition = new Vector3(0, levelPosition + LookAheadYValue, GroundZValue);
			BaseGame.ViewMatrix = Matrix.CreateLookAt(
				cameraPosition,
				lookAtPosition,
				new Vector3(0, 1, 0));

			if (Player.GameOver)
			{
				cameraPosition += new Vector3(0, 0, -20) +
					Vector3.TransformNormal(new Vector3(30, 0, 0),
					Matrix.CreateRotationZ(BaseGame.TotalTimeMs / 2593.0f));
				BaseGame.ViewMatrix = Matrix.CreateLookAt(
					cameraPosition,
					lookAtPosition,
					new Vector3(0, 1, 0));
			} // if
			#endregion

			#region Update landscape position
			// Show current landscape block and the next one with a wall
			// segment between them. We usually just see one block.
			int blockPosition = (int)((levelPosition + LookAheadYValue) / SegmentLength);
			if (blockPosition + 1 != generatedLandscapeSegmentNumber)
			{
				// Copy over last objects
				thisLandscapeSegmentObjects = nextLandscapeSegmentObjects;
				nextLandscapeSegmentObjects =
					GenerateLandscapeSegment(blockPosition + 1);
			} // if
			#endregion

			#region Prepare rendering
			Vector3 levelVector = new Vector3(0, levelPosition + LookAheadYValue - 5, 0);
			// Start new list
			numOfModelsToRender = 0;
			#endregion

			#region Render landscape
			Model landscapeModel = landscapeModels[(int)0];
			Matrix landscapeScaleMatrix = Matrix.CreateScale(
				LandscapeModelSize[(int)0]);
			AddModelToRender(landscapeModel, landscapeScaleMatrix *
				Matrix.CreateTranslation(
				new Vector3(0, blockPosition * SegmentLength, 0)));
			AddModelToRender(landscapeModel, landscapeScaleMatrix *
				Matrix.CreateTranslation(
				new Vector3(0, blockPosition * SegmentLength + SegmentLength, 0)));
			#endregion

			#region Render ships
			Player.shipPos =
				new Vector3(Player.position, AllShipsZHeight) + levelVector;
			AddModelToRender(
				shipModels[(int)ShipModelTypes.OwnShip],
				Matrix.CreateScale(ShipModelSize[(int)ShipModelTypes.OwnShip]) *
				Matrix.CreateRotationZ(MathHelper.Pi) *
				Matrix.CreateRotationX(Player.shipRotation.Y) *
				Matrix.CreateRotationY(Player.shipRotation.X) *
				Matrix.CreateTranslation(Player.shipPos));
			// Add smoke effects for our ship
			EffectManager.AddRocketOrShipFlareAndSmoke(
				Player.shipPos + new Vector3(-0.3f, -2.65f, +0.35f), 1.35f,
				5 * Player.MovementSpeedPerSecond);
			EffectManager.AddRocketOrShipFlareAndSmoke(
				Player.shipPos + new Vector3(0.3f, -2.65f, +0.35f), 1.35f,
				5 * Player.MovementSpeedPerSecond);

			// Render enemy units and all weapon projectiles
			RenderEnemyUnits();
			RenderWeaponProjectiles();
			RenderItems();
			#endregion

			#region Render landscape objects
			// Show all landscape models
			// Note: Avoid foreach to optimize performance on Xbox 360!
			for (int num = 0; num < thisLandscapeSegmentObjects.Count; num++)
			{
				MatrixAndNumber obj = thisLandscapeSegmentObjects[num];
				AddModelToRender(landscapeModels[obj.number], obj.renderMatrix);
			} // for
			for (int num = 0; num < nextLandscapeSegmentObjects.Count; num++)
			{
				MatrixAndNumber obj = nextLandscapeSegmentObjects[num];
				AddModelToRender(landscapeModels[obj.number], obj.renderMatrix);
			} // for
			#endregion

			#region Render all and add shadows
			#region Generate shadows
			// Generate shadows
			ShaderEffect.shadowMapping.GenerateShadows(
				delegate
				{
					// Generate shadows for all models except the first two (landscape)
					for (int num = 2; num < numOfModelsToRender; num++)
						modelsToRender[num].model.GenerateShadow(
							modelsToRender[num].matrix);
				});

			// Render shadows
			ShaderEffect.shadowMapping.RenderShadows(
				delegate
				{
					// Show and render shadows for all models including the landscape
					for (int num = 0; num < numOfModelsToRender; num++)
						modelsToRender[num].model.UseShadow(
							modelsToRender[num].matrix);
				});
			#endregion

			#region Render normally
			// Time to render all models the normal way (with normal mapping)
			for (int num = 0; num < numOfModelsToRender; num++)
				modelsToRender[num].model.Render(
					modelsToRender[num].matrix);

			// And finally bring everything on the screen
			BaseGame.MeshRenderManager.Render();
			#endregion

			#region Show shadows
			ShaderEffect.shadowMapping.ShowShadows();
			#endregion
			#endregion
		} // RenderLevelBackground()
		#endregion

		#region Render hud
		private void RenderHud()
		{
			Rectangle rect =
				BaseGame.CalcRectangleKeep4To3(hudTopTexture.GfxRectangle);
#if XBOX360
			// Make it 7% smaller!
			rect.X = (int)(rect.X + BaseGame.Width * 0.035f);
			rect.Y = (int)(rect.Y + BaseGame.Height * 0.035f);
			rect.Width = (int)(rect.Width * 0.93f);
			//rect.Height = (int)(rect.Height * 0.93f);
#endif
			// Render top hud part
			hudTopTexture.RenderOnScreen(
				rect, hudTopTexture.GfxRectangle);

			// Time
			BaseGame.NumbersFont.WriteTime(
				rect.X + (int)(73 * rect.Width / 1024.0f),
				rect.Y + BaseGame.YToRes(8),
				(int)Player.gameTimeMs);
			// Score
			BaseGame.NumbersFont.WriteNumberCentered(
				rect.X + (int)(485 * rect.Width / 1024.0f),
				rect.Y + BaseGame.YToRes(8),
				Player.score);
			// Highscore
			BaseGame.NumbersFont.WriteNumberCentered(
				rect.X + (int)(920 * rect.Width / 1024.0f),
				rect.Y + BaseGame.YToRes(8),
				Highscores.TopHighscore);

			// Render bottom hud part
			Rectangle bottomHudGfxRect = new Rectangle(0, 25, 1024, 39);
			rect = BaseGame.CalcRectangleKeep4To3(bottomHudGfxRect);
			rect.Y = BaseGame.YToRes(768-40);
#if XBOX360
			// Make it 10% smaller!
			rect.X = (int)(rect.X + BaseGame.Width * 0.035f);
			rect.Y = (int)(rect.Y - BaseGame.Height * 0.035f);
			rect.Width = (int)(rect.Width * 0.93f);
			//rect.Height = (int)(rect.Height * 0.93f);
#endif
			hudBottomTexture.RenderOnScreen(
				rect, bottomHudGfxRect);
			// Health
			//tst: Player.health = 0.35f;
			Rectangle healthGfxRect = new Rectangle(50, 0, 361, 24);
			healthGfxRect = new Rectangle(healthGfxRect.X, healthGfxRect.Y,
				(int)(healthGfxRect.Width * Player.health), healthGfxRect.Height);
			hudBottomTexture.RenderOnScreen(
				//Relative4To3(50, 768 - 31,
				new Rectangle(
				rect.X + (int)(50 * rect.Width / 1024.0f),
				rect.Bottom - BaseGame.YToRes(31),
				(int)(healthGfxRect.Width * rect.Width / 1024.0f),
				BaseGame.YToRes(24)),
				healthGfxRect);
			// Weapon and Emps!
			Rectangle weaponMgGfxRect = new Rectangle(876, 0, 31, 24);
			Rectangle weaponGattlingGfxRect = new Rectangle(909, 0, 27, 24);
			Rectangle weaponPlasmaGfxRect = new Rectangle(939, 0, 33, 24);
			Rectangle weaponRocketsGfxRect = new Rectangle(975, 0, 24, 24);
			Rectangle weaponEmpGfxRect = new Rectangle(1001, 0, 23, 24);
			int xPos = rect.X + (int)(608 * rect.Width / 1024.0f);
			int yPos = rect.Bottom - BaseGame.YToRes(20) -
				TextureFont.Height / 3;
			//TextureFont.WriteText(xPos, yPos, "Weapon: ");
			// Show level pos instead (in percentage)!
			TextureFont.WriteText(xPos, yPos,
				(int)(100 * Player.gameTimeMs /
				(64.0f * TotalSegments * SegmentLength)) + "%");
			// Show weapon icon!
			Rectangle weaponRect =
				Player.currentWeapon == Player.WeaponTypes.MG ? weaponMgGfxRect :
				Player.currentWeapon == Player.WeaponTypes.Gattling ? weaponGattlingGfxRect :
				Player.currentWeapon == Player.WeaponTypes.Plasma ? weaponPlasmaGfxRect :
				weaponRocketsGfxRect;
			hudBottomTexture.RenderOnScreen(
				new Rectangle(
				rect.X + (int)(704 * rect.Width / 1024.0f),
				rect.Bottom - BaseGame.YToRes(30),
				(int)((weaponRect.Width-2) * rect.Width / 1024.0f),
				BaseGame.YToRes(weaponRect.Height-2)),
				weaponRect);
			// And weapon name
			xPos += BaseGame.XToRes(735 - 606);
			TextureFont.WriteText(xPos, yPos, Player.currentWeapon.ToString());
			xPos += BaseGame.XToRes(858 - 747);
			TextureFont.WriteText(xPos, yPos, "EMPs: ");
			// Show emp icons if we have any
			//tst:Player.empBombs = 3;
			xPos = rect.Right - BaseGame.XToRes(1024 - 916);
			yPos = rect.Y + BaseGame.YToRes(8);// (int)((768 - 31) * rect.Height / 768.0f);
			for (int num = 0; num < Player.empBombs; num++)
				hudBottomTexture.RenderOnScreen(
					new Rectangle(xPos+BaseGame.XToRes(24*num), yPos,
					BaseGame.XToRes(weaponEmpGfxRect.Width),
					BaseGame.XToRes(weaponEmpGfxRect.Height)),
					weaponEmpGfxRect);
		} // RenderHud()
		#endregion

		#region Run
		/// <summary>
		/// Run game screen. Called each frame.
		/// </summary>
		/// <param name="game">Form for access to asteroid manager and co</param>
		public void Run(XnaShooterGame game)
		{
			// Make sure the textures and models are linked correctly
			hudTopTexture = game.hudTopTexture;
			hudBottomTexture = game.hudBottomTexture;
			landscapeModels = game.landscapeModels;
			shipModels = game.shipModels;
			itemModels = game.itemModels;

			// Render landscape, ships and neutral objects
			RenderLevelBackground(
				//tst: 30+
				Player.gameTimeMs / 64.0f);//66.0f);//33.0f);

			// Handle game logic, move player around, weapons, collisions, etc.
			Player.HandleGameLogic(this);

			// Handle effect stuff
			BaseGame.effectManager.HandleAllEffects();

			// Render hud on top of 3d stuff
			RenderHud();

			// End game if escape was pressed or game is over and space or mouse
			// button was pressed.
			if (Input.KeyboardEscapeJustPressed ||
				Input.GamePadBackJustPressed ||
				(Player.GameOver &&
				(Input.KeyboardSpaceJustPressed ||
				Input.GamePadAJustPressed ||
				Input.GamePadBJustPressed ||
				Input.GamePadXJustPressed ||
				Input.GamePadXJustPressed ||
				Input.MouseLeftButtonJustPressed)))
			{
				// Upload new highscore (as we currently are in game,
				// no bonus or anything will be added, this score is low!)
				Player.SetGameOverAndUploadHighscore();

				// Reset camera to origin and notify we are not longer in game mode
				XnaShooterGame.camera.SetPosition(Vector3.Zero);

				// Quit to the main menu
				quit = true;
			} // if
		} // Run(game)
		#endregion

		#region Unit Testing
#if DEBUG
		/// <summary>
		/// Test render level background with landscape objects.
		/// </summary>
		public static void TestRenderLevelBackground()
		{
			Mission dummyMission = null;

			TestGame.Start("TestRenderLevelBackground",
				delegate
				{
					dummyMission = new Mission();
				},
				delegate
				{
					dummyMission.RenderLevelBackground(
						BaseGame.TotalTimeMs / 33.0f);
				});
		} // TestRenderLevelBackground()

		/// <summary>
		/// Test hud
		/// </summary>
		public static void TestHud()
		{
			Mission dummyMission = null;

			TestGame.Start("TestHud",
				delegate
				{
					dummyMission = new Mission();
					dummyMission.hudTopTexture = new Texture("HudTop");
					dummyMission.hudBottomTexture = new Texture("HudBottom");
				},
				delegate
				{
					dummyMission.RenderLevelBackground(
						BaseGame.TotalTimeMs / 33.0f);

					dummyMission.RenderHud();
				});
		} // TestHud()
#endif
		#endregion
	} // class Mission
} // namespace XnaShooter.GameScreens
