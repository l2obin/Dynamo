﻿using Autodesk.Revit.DB;
using Dynamo.Utilities;
using NUnit.Framework;
using RevitServices;
using System;

namespace RevitServicesTests
{
    [TestFixture]
    public class RevitServicesTests
    {
        Document Document
        {
            get { return dynRevitSettings.Doc.Document; }
        }

        [Test]
        public void MakePoint()
        {
            var transManager = new TransactionManager();
            var t = transManager.StartTransaction(Document);

            var id = Document.FamilyCreate.NewReferencePoint(new XYZ(0, 0, 0)).Id;

            t.CommitTransaction();

            ReferencePoint rp;
            Assert.IsTrue(Document.TryGetElement(id, out rp));
            Assert.AreEqual(id, rp.Id);
        }

        [Test]
        public void MakePointThenCancel()
        {
            var transManager = new TransactionManager();
            var t = transManager.StartTransaction(Document);

            var id = Document.FamilyCreate.NewReferencePoint(new XYZ(0, 0, 0)).Id;

            t.CancelTransaction();

            ReferencePoint rp;
            Assert.IsFalse(Document.TryGetElement(id, out rp));
        }

        [Test]
        public void TransactionStartedEventFires()
        {
            bool eventWasFired = false;

            var transManager = new TransactionManager();
            transManager.TransactionStarted += delegate { eventWasFired = true; };

            Assert.IsFalse(eventWasFired);
            var t = transManager.StartTransaction(Document);

            Assert.IsTrue(eventWasFired);
            t.CancelTransaction();
        }

        [Test]
        public void TransactionCommittedEventFires()
        {
            bool eventWasFired = false;

            var transManager = new TransactionManager();
            transManager.TransactionCommitted += delegate { eventWasFired = true; };

            Assert.IsFalse(eventWasFired);

            var t = transManager.StartTransaction(Document);
            Assert.IsFalse(eventWasFired);

            t.CommitTransaction();
            Assert.IsTrue(eventWasFired);
        }

        [Test]
        public void TransactionCancelledEventFires()
        {
            bool eventWasFired = false;

            var transManager = new TransactionManager();
            transManager.TransactionCancelled += delegate { eventWasFired = true; };

            Assert.IsFalse(eventWasFired);

            var t = transManager.StartTransaction(Document);
            Assert.IsFalse(eventWasFired);

            t.CancelTransaction();
            Assert.IsTrue(eventWasFired);
        }

        [Test]
        public void TransactionActive()
        {
            var transManager = new TransactionManager();
            Assert.IsFalse(transManager.TransactionActive);

            var t = transManager.StartTransaction(Document);
            Assert.IsTrue(transManager.TransactionActive);

            t.CancelTransaction();
            Assert.IsFalse(transManager.TransactionActive);

            t = transManager.StartTransaction(Document);
            Assert.IsTrue(transManager.TransactionActive);

            t.CommitTransaction();
            Assert.IsFalse(transManager.TransactionActive);
        }

        [Test]
        public void TransactionHandleStatus()
        {
            var transManager = new TransactionManager();

            var t = transManager.StartTransaction(Document);
            Assert.AreEqual(TransactionStatus.Started, t.Status);

            t.CommitTransaction();

            Assert.Throws<InvalidOperationException>(
                () => t.CommitTransaction(), 
                "Cannot commit a transaction that isn't active.");

            Assert.Throws<InvalidOperationException>(
                () => t.CancelTransaction(),
                "Cannot cancel a transaction that isn't active.");
        }

        [Test]
        public void FailuresRaisedEvent()
        {
            //TODO
            Assert.Inconclusive("TODO: find an example that would cause revit to emit failures");
        }
    }
}