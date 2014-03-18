﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XervBackup.Scheduler.Monitor.SQL
{
    /// <summary>
    /// Maintains the server data
    /// </summary>
    public class Plugin : XervBackup.Scheduler.Data.IMonitorPlugin
    {
        public string Name { get { return "Monitor.SQL"; } }
        /// <summary>
        /// Update a log entry
        /// </summary>
        /// <param name="aLogDate">Date</param>
        /// <param name="aLogContent">XML content</param>
        public void UpdateLog(DateTime aLogDate, string aLogContent)
        {
            SQLHistoryDataSet.Update(XervBackup.Scheduler.Utility.User.UserName, Environment.MachineName, SQLHistoryDataSet.EntryKind.Log,
                aLogDate, aLogContent);
        }
        /// <summary>
        /// Update the scheduler record
        /// </summary>
        public void Scheduler()
        {
            UpdateScheduler(XervBackup.Scheduler.Data.SchedulerDataSet.DefaultPath());
        }
        /// <summary>
        /// Update the scheduler record
        /// </summary>
        /// <param name="aScheduler">XML file to load</param>
        public void UpdateScheduler(string aScheduler)
        {
            DateTime LastUpdate = SQLHistoryDataSet.Latest(SQLHistoryDataSet.EntryKind.Schedule);
            DateTime LastWrite = System.IO.File.GetLastWriteTime(aScheduler);
            if (LastUpdate < LastWrite)
                SQLHistoryDataSet.Update(XervBackup.Scheduler.Utility.User.UserName, Environment.MachineName, SQLHistoryDataSet.EntryKind.Schedule,
                    LastWrite, System.IO.File.ReadAllText(aScheduler));
        }
        /// <summary>
        /// Update History
        /// </summary>
        public void UpdateHistory(string aHistory)
        {
            using (XervBackup.Scheduler.Data.HistoryDataSet hds = new XervBackup.Scheduler.Data.HistoryDataSet())
            {
                hds.Load();
                History(System.IO.File.GetLastWriteTime(aHistory), hds.History);
            }
        }
        /// <summary>
        /// Update History
        /// </summary>
        /// <param name="aLastWrite">Date</param>
        /// <param name="aHistoryTable">XML file name</param>
        public void History(DateTime aLastWrite, XervBackup.Scheduler.Data.HistoryDataSet.HistoryDataTable aHistoryTable)
        {
            DateTime LastUpdate = SQLHistoryDataSet.Latest(SQLHistoryDataSet.EntryKind.Schedule);
            XervBackup.Scheduler.Data.HistoryDataSet.HistoryDataTable Changes = new XervBackup.Scheduler.Data.HistoryDataSet.HistoryDataTable();
            foreach (XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow Row in
                from XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow qR in aHistoryTable
                where qR.ActionDate > LastUpdate
                select qR)
                Changes.ImportRow(Row);
            StringBuilder b = new StringBuilder();
            Changes.WriteXml(new System.IO.StringWriter(b));
            SQLHistoryDataSet.Update(XervBackup.Scheduler.Utility.User.UserName, Environment.MachineName, SQLHistoryDataSet.EntryKind.History,
                aLastWrite, b.ToString());
        }
        public System.Windows.Forms.DialogResult Configure()
        {
            return new SQLConnectionDialog(Properties.Settings.MonitorConnection).ShowDialog();
        }
    }
}
