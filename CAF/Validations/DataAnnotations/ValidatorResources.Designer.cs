﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAF.Validations.DataAnnotations {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidatorResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidatorResources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CAF.Validations.DataAnnotations.ValidatorResources", typeof(ValidatorResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 A value for &apos;{0}&apos; is required but was not present in the request. 的本地化字符串。
        /// </summary>
        public static string BindingBehavior_ValueNotFound {
            get {
                return ResourceManager.GetString("BindingBehavior_ValueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The &apos;Duration&apos; property must be a positive number. 的本地化字符串。
        /// </summary>
        public static string ChildActionCacheAttribute_DurationMustBePositive {
            get {
                return ResourceManager.GetString("ChildActionCacheAttribute_DurationMustBePositive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 This model binder does not support the model type &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        public static string Common_ModelBinderDoesNotSupportModelType {
            get {
                return ResourceManager.GetString("Common_ModelBinderDoesNotSupportModelType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value cannot be null or empty. 的本地化字符串。
        /// </summary>
        public static string Common_NullOrEmpty {
            get {
                return ResourceManager.GetString("Common_NullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The property {0}.{1} could not be found. 的本地化字符串。
        /// </summary>
        public static string Common_PropertyNotFound {
            get {
                return ResourceManager.GetString("Common_PropertyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The type &apos;{0}&apos; does not implement the interface &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string Common_TypeMustImplementInterface {
            get {
                return ResourceManager.GetString("Common_TypeMustImplementInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The &apos;Name&apos; property must be set. 的本地化字符串。
        /// </summary>
        public static string CommonControls_NameRequired {
            get {
                return ResourceManager.GetString("CommonControls_NameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &apos;{0}&apos; and &apos;{1}&apos; do not match. 的本地化字符串。
        /// </summary>
        public static string CompareAttribute_MustMatch {
            get {
                return ResourceManager.GetString("CompareAttribute_MustMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The IControllerFactory &apos;{0}&apos; did not return a controller for a controller named &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string ControllerBuilder_FactoryReturnedNull {
            get {
                return ResourceManager.GetString("ControllerBuilder_FactoryReturnedNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid credit card number. 的本地化字符串。
        /// </summary>
        public static string CreditCardAttribute_Invalid {
            get {
                return ResourceManager.GetString("CreditCardAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid CUIT number. 的本地化字符串。
        /// </summary>
        public static string CuitAttribute_Invalid {
            get {
                return ResourceManager.GetString("CuitAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} has a DisplayColumn attribute for {1}, but property {1} does not exist. 的本地化字符串。
        /// </summary>
        public static string DataAnnotationsModelMetadataProvider_UnknownProperty {
            get {
                return ResourceManager.GetString("DataAnnotationsModelMetadataProvider_UnknownProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} has a DisplayColumn attribute for {1}, but property {1} does not have a public getter. 的本地化字符串。
        /// </summary>
        public static string DataAnnotationsModelMetadataProvider_UnreadableProperty {
            get {
                return ResourceManager.GetString("DataAnnotationsModelMetadataProvider_UnreadableProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} is not a valid date 的本地化字符串。
        /// </summary>
        public static string DateAttribute_Invalid {
            get {
                return ResourceManager.GetString("DateAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} should contain only digits 的本地化字符串。
        /// </summary>
        public static string DigitsAttribute_Invalid {
            get {
                return ResourceManager.GetString("DigitsAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Sample Item 的本地化字符串。
        /// </summary>
        public static string DropDownList_SampleItem {
            get {
                return ResourceManager.GetString("DropDownList_SampleItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 DynamicViewDataDictionary only supports single indexers. 的本地化字符串。
        /// </summary>
        public static string DynamicViewDataDictionary_SingleIndexerOnly {
            get {
                return ResourceManager.GetString("DynamicViewDataDictionary_SingleIndexerOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 DynamicViewDataDictionary only supports string-based indexers. 的本地化字符串。
        /// </summary>
        public static string DynamicViewDataDictionary_StringIndexerOnly {
            get {
                return ResourceManager.GetString("DynamicViewDataDictionary_StringIndexerOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The property {0} doesn&apos;t exist. There are no public properties on this object. 的本地化字符串。
        /// </summary>
        public static string DynamicViewPage_NoProperties {
            get {
                return ResourceManager.GetString("DynamicViewPage_NoProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The property {0} doesn&apos;t exist. Supported properties are: {1}. 的本地化字符串。
        /// </summary>
        public static string DynamicViewPage_PropertyDoesNotExist {
            get {
                return ResourceManager.GetString("DynamicViewPage_PropertyDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件的格式不正确，请修改 的本地化字符串。
        /// </summary>
        public static string EmailAddressAttribute_Invalid {
            get {
                return ResourceManager.GetString("EmailAddressAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not find a property named {0}. 的本地化字符串。
        /// </summary>
        public static string EqualTo_UnknownProperty {
            get {
                return ResourceManager.GetString("EqualTo_UnknownProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The method &apos;{0}&apos; is an asynchronous completion method and cannot be called directly. 的本地化字符串。
        /// </summary>
        public static string ExpressionHelper_CannotCallCompletedMethod {
            get {
                return ResourceManager.GetString("ExpressionHelper_CannotCallCompletedMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The method &apos;{0}&apos; is marked [NonAction] and cannot be called directly. 的本地化字符串。
        /// </summary>
        public static string ExpressionHelper_CannotCallNonAction {
            get {
                return ResourceManager.GetString("ExpressionHelper_CannotCallNonAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot route to class named &apos;Controller&apos;. 的本地化字符串。
        /// </summary>
        public static string ExpressionHelper_CannotRouteToController {
            get {
                return ResourceManager.GetString("ExpressionHelper_CannotRouteToController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Expression must be a method call. 的本地化字符串。
        /// </summary>
        public static string ExpressionHelper_MustBeMethodCall {
            get {
                return ResourceManager.GetString("ExpressionHelper_MustBeMethodCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Controller name must end in &apos;Controller&apos;. 的本地化字符串。
        /// </summary>
        public static string ExpressionHelper_TargetMustEndInController {
            get {
                return ResourceManager.GetString("ExpressionHelper_TargetMustEndInController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The new model binding system cannot be used when a property whitelist or blacklist has been specified in [Bind] or via the call to UpdateModel() / TryUpdateModel(). Use the [BindRequired] and [BindNever] attributes on the model type or its properties instead. 的本地化字符串。
        /// </summary>
        public static string ExtensibleModelBinderAdapter_PropertyFilterMustNotBeSet {
            get {
                return ResourceManager.GetString("ExtensibleModelBinderAdapter_PropertyFilterMustNotBeSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field only accepts files with the following extensions: {1} 的本地化字符串。
        /// </summary>
        public static string FileExtensionsAttribute_Invalid {
            get {
                return ResourceManager.GetString("FileExtensionsAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The type &apos;{0}&apos; is not an open generic type. 的本地化字符串。
        /// </summary>
        public static string GenericModelBinderProvider_ParameterMustSpecifyOpenGenericType {
            get {
                return ResourceManager.GetString("GenericModelBinderProvider_ParameterMustSpecifyOpenGenericType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The open model type &apos;{0}&apos; has {1} generic type argument(s), but the open binder type &apos;{2}&apos; has {3} generic type argument(s). The binder type must not be an open generic type or must have the same number of generic arguments as the open model type. 的本地化字符串。
        /// </summary>
        public static string GenericModelBinderProvider_TypeArgumentCountMismatch {
            get {
                return ResourceManager.GetString("GenericModelBinderProvider_TypeArgumentCountMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 There is no ViewData item with the key &apos;{0}&apos; of type &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string HtmlHelper_MissingSelectData {
            get {
                return ResourceManager.GetString("HtmlHelper_MissingSelectData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The ViewData item with the key &apos;{0}&apos; is of type &apos;{1}&apos; but needs to be of type &apos;{2}&apos;. 的本地化字符串。
        /// </summary>
        public static string HtmlHelper_WrongSelectDataType {
            get {
                return ResourceManager.GetString("HtmlHelper_WrongSelectDataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} should be a positive or negative non-decimal number. 的本地化字符串。
        /// </summary>
        public static string IntegerAttribute_Invalid {
            get {
                return ResourceManager.GetString("IntegerAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 手机号无效 的本地化字符串。
        /// </summary>
        public static string InvalidMobilePhone {
            get {
                return ResourceManager.GetString("InvalidMobilePhone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} must be less than or equal to {1} 的本地化字符串。
        /// </summary>
        public static string MaxAttribute_Invalid {
            get {
                return ResourceManager.GetString("MaxAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} must be greater than or equal to {1} 的本地化字符串。
        /// </summary>
        public static string MinAttribute_Invalid {
            get {
                return ResourceManager.GetString("MinAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The value &apos;{0}&apos; is not valid for {1}. 的本地化字符串。
        /// </summary>
        public static string ModelBinderConfig_ValueInvalid {
            get {
                return ResourceManager.GetString("ModelBinderConfig_ValueInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 A value is required. 的本地化字符串。
        /// </summary>
        public static string ModelBinderConfig_ValueRequired {
            get {
                return ResourceManager.GetString("ModelBinderConfig_ValueRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 A binder for type {0} could not be located. 的本地化字符串。
        /// </summary>
        public static string ModelBinderProviderCollection_BinderForTypeNotFound {
            get {
                return ResourceManager.GetString("ModelBinderProviderCollection_BinderForTypeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The type &apos;{0}&apos; does not subclass {1} or implement the interface {2}. 的本地化字符串。
        /// </summary>
        public static string ModelBinderProviderCollection_InvalidBinderType {
            get {
                return ResourceManager.GetString("ModelBinderProviderCollection_InvalidBinderType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The model of type &apos;{0}&apos; has a [Bind] attribute. The new model binding system cannot be used with models that have type-level [Bind] attributes. Use the [BindRequired] and [BindNever] attributes on the model type or its properties instead. 的本地化字符串。
        /// </summary>
        public static string ModelBinderProviderCollection_TypeCannotHaveBindAttribute {
            get {
                return ResourceManager.GetString("ModelBinderProviderCollection_TypeCannotHaveBindAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The binding context has a null Model, but this binder requires a non-null model of type &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        public static string ModelBinderUtil_ModelCannotBeNull {
            get {
                return ResourceManager.GetString("ModelBinderUtil_ModelCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The binding context has a Model of type &apos;{0}&apos;, but this binder can only operate on models of type &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string ModelBinderUtil_ModelInstanceIsWrong {
            get {
                return ResourceManager.GetString("ModelBinderUtil_ModelInstanceIsWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The binding context cannot have a null ModelMetadata. 的本地化字符串。
        /// </summary>
        public static string ModelBinderUtil_ModelMetadataCannotBeNull {
            get {
                return ResourceManager.GetString("ModelBinderUtil_ModelMetadataCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The binding context has a ModelType of &apos;{0}&apos;, but this binder can only operate on models of type &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string ModelBinderUtil_ModelTypeIsWrong {
            get {
                return ResourceManager.GetString("ModelBinderUtil_ModelTypeIsWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The ModelMetadata property must be set before accessing this property. 的本地化字符串。
        /// </summary>
        public static string ModelBindingContext_ModelMetadataMustBeSet {
            get {
                return ResourceManager.GetString("ModelBindingContext_ModelMetadataMustBeSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The ControllerBuilder must return an IControllerFactory of type {0} if the MvcDynamicSessionModule is enabled. 的本地化字符串。
        /// </summary>
        public static string MvcDynamicSessionModule_WrongControllerFactory {
            get {
                return ResourceManager.GetString("MvcDynamicSessionModule_WrongControllerFactory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Deserialization failed. Verify that the data is being deserialized using the same SerializationMode with which it was serialized. Otherwise see the inner exception. 的本地化字符串。
        /// </summary>
        public static string MvcSerializer_DeserializationFailed {
            get {
                return ResourceManager.GetString("MvcSerializer_DeserializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The provided SerializationMode is invalid. 的本地化字符串。
        /// </summary>
        public static string MvcSerializer_InvalidSerializationMode {
            get {
                return ResourceManager.GetString("MvcSerializer_InvalidSerializationMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The data being serialized is corrupt. 的本地化字符串。
        /// </summary>
        public static string MvcSerializer_MagicHeaderCheckFailed {
            get {
                return ResourceManager.GetString("MvcSerializer_MagicHeaderCheckFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid number. 的本地化字符串。
        /// </summary>
        public static string NumericAttribute_Invalid {
            get {
                return ResourceManager.GetString("NumericAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Error dispatching on controller {0}, conflicting actions matched: {1}. 的本地化字符串。
        /// </summary>
        public static string ResourceControllerFactory_ConflictingActions {
            get {
                return ResourceManager.GetString("ResourceControllerFactory_ConflictingActions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Error dispatching on controller {0}, no actions matched. 的本地化字符串。
        /// </summary>
        public static string ResourceControllerFactory_NoActions {
            get {
                return ResourceManager.GetString("ResourceControllerFactory_NoActions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Format &apos;{0}&apos; is not supported. 的本地化字符串。
        /// </summary>
        public static string Resources_UnsupportedFormat {
            get {
                return ResourceManager.GetString("Resources_UnsupportedFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unsupported Media Type: &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        public static string Resources_UnsupportedMediaType {
            get {
                return ResourceManager.GetString("Resources_UnsupportedMediaType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid fully-qualified http, https, or ftp URL. 的本地化字符串。
        /// </summary>
        public static string UrlAttribute_Invalid {
            get {
                return ResourceManager.GetString("UrlAttribute_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid URL. 的本地化字符串。
        /// </summary>
        public static string UrlAttributeProtocolOptional_Invalid {
            get {
                return ResourceManager.GetString("UrlAttributeProtocolOptional_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid protocol-less URL. 的本地化字符串。
        /// </summary>
        public static string UrlAttributeWithoutProtocol_Invalid {
            get {
                return ResourceManager.GetString("UrlAttributeWithoutProtocol_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The {0} field is not a valid year 的本地化字符串。
        /// </summary>
        public static string YearAttribute_Invalid {
            get {
                return ResourceManager.GetString("YearAttribute_Invalid", resourceCulture);
            }
        }
    }
}
