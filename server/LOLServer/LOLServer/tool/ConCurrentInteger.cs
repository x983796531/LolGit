﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLServer.tool
{
    public class ConCurrentInteger
    {
        int value;
        Mutex tex = new Mutex();
        public ConCurrentInteger() { }
        public ConCurrentInteger(int value) { this.value = value; }

        /// <summary>
        /// 自增并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock (this)
            {
                tex.WaitOne();
                value++;
                tex.ReleaseMutex();
                return value;
            }
        }

        /// <summary>
        /// 自减并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                tex.WaitOne();
                value--;
                tex.ReleaseMutex();
                return value;
            }
        }

        /// <summary>
        /// 重置value为0
        /// </summary>
        public void reset()
        {
            lock(this)
            {
                tex.WaitOne();
                value = 0;
                tex.ReleaseMutex();
            }
        }

        public int get()
        {
            return value;
        }
    }
}
