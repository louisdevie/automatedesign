﻿using AutomateDesign.Core.Random;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Une demande d'inscription.
    /// </summary>
    public class Registration
    {
        private uint verificationCode;
        private DateTime expiration;
        private User user;

        /// <summary>
        /// Le code de vérification associé à cette demande d'inscription.
        /// </summary>
        public uint VerificationCode => this.verificationCode;

        /// <summary>
        /// La durée maximum d'une demande d'inscription (2 heures).
        /// </summary>
        public static readonly TimeSpan LIFETIME = TimeSpan.FromHours(2);

        /// <summary>
        /// Le moment auquel la demande d'inscription expirera.
        /// </summary>
        public DateTime Expiration => this.expiration;

        /// <summary>
        /// Indique si la session a expiré ou non, en prenant en compte l'inactivité.
        /// </summary>
        public bool Expired => this.expiration < DateTime.UtcNow;

        /// <summary>
        /// L'utilisateur concerné par la demande d'inscription.
        /// </summary>
        public User User => this.user;

        /// <summary>
        /// Crée une demande d'inscription existante.
        /// </summary>
        /// <param name="verificationCode">Le code de vérification.</param>
        /// <param name="user">L'utilisateru qui demande à s'inscrire.</param>
        public Registration(uint verificationCode, DateTime expiration, User user)
        {
            this.verificationCode = verificationCode;
            this.expiration = expiration;
            this.user = user;
        }

        /// <summary>
        /// Crée une nouvelle demande d'inscription.
        /// </summary>
        /// <param name="user">L'utilisateur pour qui créer la demande d'inscription.</param>
        public Registration(User user)
        {
            this.verificationCode = new BasicRandomProvider().FourDigitCode();
            this.expiration = DateTime.UtcNow + LIFETIME;
            this.user = user;
        }

        public Registration WithUser(User user) => new(this.verificationCode, this.expiration, user);
    }
}
