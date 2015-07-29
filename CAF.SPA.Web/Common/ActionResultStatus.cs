using System;

namespace CAF.SPA.Web.Common
{
    public enum ActionStatuses
    {
        OK = 0,
        Error = 1
    }

    public class ActionResultStatus
    {
        public ActionResultStatus()
        {
            this.Status = ActionStatuses.OK;
        }
        public ActionResultStatus(int errorCode = 0, string errorMessage = "")
        {
            this.Status = ActionStatuses.Error;
            this.Message = errorMessage;
            this.ErrorCode = errorCode;
        }

        public ActionResultStatus(Exception ex)
        {
            this.Status = ActionStatuses.Error;
            this.Message = ex.Message;
            this.ErrorCode = 100;
        }
        public ActionStatuses Status { get; set; }
        public String Message { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; protected set; }
    }

    public class ActionResultData<T> : ActionResultStatus
    {

        public ActionResultData() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="status"></param>
        /// <param name="errorCode">100:默认为系统异常，其它数值用于需要时使用switch过滤</param>
        /// <param name="errorMessage"></param>
        public ActionResultData(T result, ActionStatuses status = ActionStatuses.OK, int errorCode = 0, string errorMessage = "")
        {
            this.Data = result;
            base.Status = status;
            base.Message = errorMessage;
            base.ErrorCode = errorCode;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode">100:默认为系统异常，其它数值用于需要时使用switch过滤</param>
        /// <param name="errorMessage"></param>
        public ActionResultData(int errorCode = 0, string errorMessage = "")
        {
            this.Data = default(T);
            base.Status = ActionStatuses.Error;
            base.Message = errorMessage;
            base.ErrorCode = errorCode;
        }

        public ActionResultData(Exception ex)
            : base(ex) { }


        public T Data { get; set; }
    }
}