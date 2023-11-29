using AutomateDesign.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Users
{
    public class SessionTest
    {
        private string token = "Token";
        private DateTime lastUse = DateTime.Today;
        private DateTime expiration = DateTime.Now;
        private User user = new User(1, "test.email@iut-dijon.u-bourgogne.fr", HashedPassword.FromPlain("12345678"), true);

        private Session createSession()
        {
            return new Session(token, lastUse, expiration, user);
        }

        [Fact]
        public void TestCreateSession()
        {
            Session session = createSession();

            Assert.Equal(token, session.Token);
            Assert.Equal(lastUse, session.LastUse);
            Assert.Equal(expiration, session.Expiration);
            Assert.Equal(user, session.User);
        }

        [Fact]
        public void TestToken()
        {
            Session session = createSession();
            Assert.Equal(session.Token, token);
        }

        [Fact]
        public void TestUser()
        {
            Session session = createSession();
            Assert.Equal(session.User, user);
        }

        [Fact]
        public void TestLastUs()
        {
            Session session = createSession();
            Assert.Equal(session.LastUse, lastUse);
        }

        [Fact]
        public void TestExpiration()
        {
            Session session = createSession();
            Assert.Equal(session.Expiration, expiration);
        }

        [Fact]
        public void TestGetExpired()
        {
            // Date d'expiration dépassé
            Session session = new Session(token, lastUse, DateTime.MinValue, user);
            Assert.True(session.Expired);

            // lastUse > 30 minutes
            session = new Session(token, lastUse, DateTime.MaxValue, user);
            Assert.True(session.Expired);

            // Session valide
            session = new Session(token, DateTime.Now, DateTime.MaxValue, user);
            Assert.False(session.Expired);
        }

        [Fact]
        public void TestRefresh()
        {
            // Session valide
            Session session = new Session(token, DateTime.UtcNow, DateTime.MaxValue, user);
            bool result = session.Refresh();

            Assert.True(result);

            // session expiré
            session = new Session(token, lastUse, DateTime.MinValue, user);
            result = session.Refresh();

            Assert.False(result);

            // Vérification des valeurs
            lastUse = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15));
            session = new Session(token, lastUse, DateTime.MaxValue, user);
            Assert.True(session.Refresh());
            DateTime updatedLastUse = session.LastUse;

            Assert.NotEqual(lastUse, updatedLastUse);
            Assert.True(updatedLastUse > lastUse);
        }

        [Fact]
        public void TestUnusedSince()
        {
            DateTime lastUse = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15));
            DateTime expiration = DateTime.MaxValue;
            Session session = new Session(token, lastUse, expiration, user);
            
            TimeSpan expectedUnusedTime = DateTime.UtcNow.Subtract(lastUse);
            int valueMeyhode = (int)Math.Round(session.UnusedSince.TotalMinutes);
            int valueExpected = (int)Math.Round(expectedUnusedTime.TotalMinutes);


            Assert.Equal(valueExpected ,valueMeyhode);
        }
    }
}
