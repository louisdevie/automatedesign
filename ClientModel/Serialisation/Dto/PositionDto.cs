using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Représente une position à transférer vers/depuis le serveur.
    /// </summary>
    public class PositionDto
    {
        /// <inheritdoc cref="Position.Top"/>
        public float Top { get; set; }

        /// <inheritdoc cref="Position.Left"/>
        public float Left { get; set; }

        /// <inheritdoc cref="Position.Width"/>
        public float Width { get; set; }

        /// <inheritdoc cref="Position.Height"/>
        public float Height { get; set; }

        /// <summary>
        /// Crée un DTO à partir d'un objet métier <see cref="Position"/>.
        /// </summary>
        /// <param name="position">L'objet métier à transférer.</param>
        /// <returns>Un nouveau <see cref="PositionDto"/>.</returns>
        internal static PositionDto MapFromModel(Position position)
        {
            return new PositionDto
            {
                Top = position.Top,
                Left = position.Left,
                Width = position.Width,
                Height = position.Height,
            };
        }

        /// <summary>
        /// Construit un objet métier <see cref="Position"/> à partir des informations de ce DTO.
        /// </summary>
        /// <returns>Une nouvelle <see cref="Position"/>.</returns>
        internal Position MapToModel()
        {
            return new Position(this.Left, this.Top, this.Width, this.Height);
        }
    }
}