using CAF.Validations;

namespace CAF.Domains.Tests.Validations {
    /// <summary>
    /// 异常验证处理 - 什么也不做
    /// </summary>
    public class NothingValidationHandler : IValidationHandler {
        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="results">验证结果集合</param>
        public void Handle( ValidationResultCollection results ) {
        }
    }
}
