using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    // l'emplacement d'un élément de l'automate
    public readonly struct Position
    {
        private readonly float top;
        private readonly float left;
        private readonly float width;
        private readonly float height;

        /// <summary>
        /// Le haut de l'élément.
        /// </summary>
        public float Top => this.top;

        /// <summary>
        /// La gauche de l'élément.
        /// </summary>
        public float Left => this.left;

        /// <summary>
        /// La largeur de l'élément.
        /// </summary>
        public float Width => this.width;

        /// <summary>
        /// La hauteur de l'élément.
        /// </summary>
        public float Height => this.height;

        /// <summary>
        /// Le bas de l'élément.
        /// </summary>
        public float Bottom => this.top + this.height;

        /// <summary>
        /// La droite de l'élément.
        /// </summary>
        public float Right => this.left + this.width;

        /// <summary>
        /// L'abscisse du centre de l'élément.
        /// </summary>
        public float CenterX => this.left + (this.width / 2);

        /// <summary>
        /// L'ordonnée du centre de l'élément.
        /// </summary>
        public float CenterY => this.top + (this.height / 2);

        /// <summary>
        /// Le centre de l'élément.
        /// </summary>
        public Vector2 Center => new(this.CenterX, this.CenterY);

        /// <summary>
        /// Crée un simple point.
        /// </summary>
        /// <param name="x">L'abscisse du point.</param>
        /// <param name="y">L'ordonnée du point.</param>
        public Position(float x, float y)
        {
            this.left = x;
            this.top = y;
            this.width = 0;
            this.height = 0;
        }

        /// <summary>
        /// Crée une position avec une forme.
        /// </summary>
        /// <param name="left">La bordure gauche de l'élément.</param>
        /// <param name="top">La bordure haute de l'élément.</param>
        /// <param name="width">La largueur prise par l'élément.</param>
        /// <param name="height">La hauteur prise par l'élément.</param>
        public Position(float left, float top, float width, float height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public Position MoveBy(float xOffset, float yOffset)
        {
            return new(this.left + xOffset, this.top + yOffset, this.width, this.height);
        }
    }
}
