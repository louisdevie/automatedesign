﻿using System.Net.Mail;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Un utilisateur de l'application.
    /// </summary>
    public class User
    {
        private int id;
        private MailAddress email;
        private HashedPassword password;

        /// <summary>
        /// L'identifiant unique de l'utilisateur.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// L'adresse mail de l'utilisateur.
        /// </summary>
        public MailAddress Email => email;

        /// <summary>
        /// Le mot de passe de l'utilisateur.
        /// </summary>
        public HashedPassword Password { get => password; set => password = value; }

        /// <summary>
        /// Crée un utilisateur existant.
        /// </summary>
        /// <param name="id">L'identifiant unique de l'utilisateur.</param>
        /// <param name="email">L'adresse mail de l'utilisateur.</param>
        /// <param name="password">Le mot de passe de l'utilisateur.</param>
        public User(int id, MailAddress email, HashedPassword password)
        {
            this.id = id;
            this.email = email;
            this.password = password;
        }

        /// <inheritdoc cref="User(int, MailAddress, HashedPassword)"/>
        public User(int id, string email, HashedPassword password)
        {
            this.id = id;
            this.email = new MailAddress(email);
            this.password = password;
        }

        /// <summary>
        /// Crée un nouvel utilisateur.
        /// </summary>
        /// <param name="email">L'adresse mail de l'utilisateur.</param>
        /// <param name="password">Le mot de passe de l'utilisateur.</param>
        public User(MailAddress email, HashedPassword password) : this(-1, email, password) { }

        /// <inheritdoc cref="User(MailAddress, HashedPassword)"/>
        public User(string email, HashedPassword password) : this(-1, email, password) { }
    }
}
