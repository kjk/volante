namespace Volante
{
    using System;
    using System.Threading;
    using System.Collections;

    /// <summary>Base class for persistent capable objects supporting locking
    /// </summary>
    public class PersistentResource : Persistent, IResource
    {
        public void SharedLock()
        {
            lock (this)
            {
                Thread currThread = Thread.CurrentThread;
                while (true)
                {
                    if (owner == currThread)
                    {
                        nWriters += 1;
                        break;
                    }
                    else if (nWriters == 0)
                    {
                        if (nReaders == 0 && db != null)
                        {
                            db.lockObject(this);
                        }
                        nReaders += 1;
                        break;
                    }
                    else
                    {
                        Monitor.Wait(this);
                    }
                }
            }
        }

        public bool SharedLock(long timeout)
        {
            Thread currThread = Thread.CurrentThread;
            DateTime startTime = DateTime.Now;
            TimeSpan ts = TimeSpan.FromMilliseconds(timeout);
            lock (this)
            {
                while (true)
                {
                    if (owner == currThread)
                    {
                        nWriters += 1;
                        return true;
                    }
                    else if (nWriters == 0)
                    {
                        if (nReaders == 0 && db != null)
                        {
                            db.lockObject(this);
                        }
                        nReaders += 1;
                        return true;
                    }
                    else
                    {
                        DateTime currTime = DateTime.Now;
                        if (startTime + ts <= currTime)
                        {
                            return false;
                        }
                        Monitor.Wait(this, startTime + ts - currTime);
                    }
                }
            }
        }


        public void ExclusiveLock()
        {
            Thread currThread = Thread.CurrentThread;
            lock (this)
            {
                while (true)
                {
                    if (owner == currThread)
                    {
                        nWriters += 1;
                        break;
                    }
                    else if (nReaders == 0 && nWriters == 0)
                    {
                        nWriters = 1;
                        owner = currThread;
                        if (db != null)
                        {
                            db.lockObject(this);
                        }
                        break;
                    }
                    else
                    {
                        Monitor.Wait(this);
                    }
                }
            }
        }

        public bool ExclusiveLock(long timeout)
        {
            Thread currThread = Thread.CurrentThread;
            TimeSpan ts = TimeSpan.FromMilliseconds(timeout);
            DateTime startTime = DateTime.Now;
            lock (this)
            {
                while (true)
                {
                    if (owner == currThread)
                    {
                        nWriters += 1;
                        return true;
                    }
                    else if (nReaders == 0 && nWriters == 0)
                    {
                        nWriters = 1;
                        owner = currThread;
                        if (db != null)
                        {
                            db.lockObject(this);
                        }
                        return true;
                    }
                    else
                    {
                        DateTime currTime = DateTime.Now;
                        if (startTime + ts <= currTime)
                        {
                            return false;
                        }
                        Monitor.Wait(this, startTime + ts - currTime);
                    }
                }
            }
        }

        public void Unlock()
        {
            lock (this)
            {
                if (nWriters != 0)
                {
                    if (--nWriters == 0)
                    {
                        owner = null;
                        Monitor.PulseAll(this);
                    }
                }
                else if (nReaders != 0)
                {
                    if (--nReaders == 0)
                    {
                        Monitor.PulseAll(this);
                    }
                }
            }
        }

        public void Reset()
        {
            lock (this)
            {
                nReaders = 0;
                nWriters = 0;
                owner = null;
                Monitor.PulseAll(this);
            }
        }

        internal protected PersistentResource() { }

        internal protected PersistentResource(IDatabase db)
            : base(db) { }

        [NonSerialized()]
        private Thread owner;
        [NonSerialized()]
        private int nReaders;
        [NonSerialized()]
        private int nWriters;
    }
}
