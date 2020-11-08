﻿using Domain.Repositories;

namespace Infrastructure
{
    /// <summary>
    /// デフォルトのインフラストラクチャの設定クラス
    /// </summary>
    public static class DefaultInfrastructure
    {
        /// <summary>
        /// DataOutputインフラストラクチャのデフォルト
        /// </summary>
        /// <returns>エクセル出力</returns>
        public static IDataOutput GetDefaultDataOutput()
        {
            return new ExcelOutputInfrastructure();
        }
        /// <summary>
        /// DataBaseConnectインフラストラクチャのデフォルト
        /// </summary>
        /// <returns>SQLServer接続</returns>
        public static IDataBaseConnect GetDefaultDataBaseConnect()
        {
            return new SQLServerConnectInfrastructure();
        }
    }
}