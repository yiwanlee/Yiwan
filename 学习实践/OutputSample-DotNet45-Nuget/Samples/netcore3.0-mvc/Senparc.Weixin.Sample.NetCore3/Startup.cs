using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Senparc.CO2NET;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Cache.Memcached;//DPBMARK Memcached DPBMARK_END
using Senparc.CO2NET.Utilities;
using Senparc.NeuChar.MessageHandlers;
using Senparc.WebSocket;//DPBMARK WebSocket DPBMARK_END
using Senparc.Weixin.Cache.Memcached;//DPBMARK Memcached DPBMARK_END
using Senparc.Weixin.Cache.Redis;//DPBMARK Redis DPBMARK_END
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;//DPBMARK MP DPBMARK_END
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;
using Senparc.Weixin.MP.Sample.CommonService.MessageHandlers.WebSocket;
using Senparc.Weixin.MP.Sample.CommonService.WorkMessageHandlers;
using Senparc.Weixin.MP.Sample.CommonService.WxOpenMessageHandler;
using Senparc.Weixin.Open;//DPBMARK Open DPBMARK_END
using Senparc.Weixin.Open.ComponentAPIs;//DPBMARK Open DPBMARK_END
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.Sample.NetCore3.WebSocket.Hubs;//DPBMARK WebSocket DPBMARK_END
using Senparc.Weixin.TenPay;//DPBMARK TenPay DPBMARK_END
using Senparc.Weixin.Work;//DPBMARK Work DPBMARK_END
using Senparc.Weixin.Work.MessageHandlers.Middleware;
using Senparc.Weixin.WxOpen;//DPBMARK MiniProgram DPBMARK_END

namespace Senparc.Weixin.Sample.NetCore3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();//ʹ��Session��ʵ��֤����Ҫ������ Mvc ֮ǰ��

            services.AddControllersWithViews()
                    .AddNewtonsoftJson()// ֧�� NewtonsoftJson
                    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            // Add CookieTempDataProvider after AddMvc and include ViewFeatures.
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            //���������linuxϵͳ�ϣ���Ҫ������������ã�
            //services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            //���������IIS�ϣ���Ҫ������������ã�
            services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();//ʹ�ñ��ػ���������

            services.AddSignalR();//ʹ�� SignalR   -- DPBMARK WebSocket DPBMARK_END

            /*
             * CO2NET �Ǵ� Senparc.Weixin ����ĵײ㹫������ģ�飬�����˳��� 6 ��ĵ����Ż����ȶ��ɿ���
             * ���� CO2NET ��������Ŀ�е�ͨ�����ÿɲο� CO2NET �� Sample��
             * https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
             */

            services.AddSenparcWeixinServices(Configuration)//Senparc.Weixin ע��
                    .AddSenparcWebSocket<CustomNetCoreWebSocketMessageHandler>();//Senparc.WebSocket ע�ᣨ���裩
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            //���� GB2312�����裩
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //����EnableRequestRewind�м�������裩
            app.UseEnableRequestRewind();
            //ʹ�� Session�����裬��ʾ������Ҫ�õ���
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //ʹ�� SignalR��.NET Core 3.0��                                                      -- DPBMARK WebSocket
            app.UseEndpoints(endpoints =>
            {
                //�����Զ��� SenparcHub
                endpoints.MapHub<SenparcHub>("/SenparcHub");
            });                                                                                  // DPBMARK_END


            // ��� CO2NET ȫ��ע�ᣬ���룡
            // ���� UseSenparcGlobal() �ĸ����÷��� CO2NET Demo��https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
            var registerService = app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
                {
                    #region CO2NET ȫ������

                    #region ȫ�ֻ������ã����裩

                    //��ͬһ���ֲ�ʽ����ͬʱ�����ڶ����վ��Ӧ�ó���أ�ʱ������ʹ�������ռ佫����루�Ǳ��룩
                    globalRegister.ChangeDefaultCacheNamespace("DefaultCO2NETCache");

                    #region ���ú�ʹ�� Redis          -- DPBMARK Redis

                    //����ȫ��ʹ��Redis���棨���裬������
                    if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//����Ϊ�˷��㲻ͬ�����Ŀ����߽������ã��������жϵķ�ʽ��ʵ�ʿ�������һ����ȷ���ģ������if�������Ժ���
                    {
                        /* ˵����
                        * 1��Redis �������ַ�����Ϣ��� Config.SenparcSetting.Cache_Redis_Configuration �Զ���ȡ��ע�ᣬ�粻��Ҫ�޸ģ��·��������Ժ���
                        /* 2�������ֶ��޸ģ�����ͨ���·� SetConfigurationOption �����ֶ����� Redis ������Ϣ�����޸����ã����������ã�
                        */
                        Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);

                        //���»�������ȫ�ֻ�������Ϊ Redis
                        Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//��ֵ�Ի�����ԣ��Ƽ���
                                                                                  //Senparc.CO2NET.Cache.Redis.Register.UseHashRedisNow();//HashSet�����ʽ�Ļ������

                        //Ҳ����ͨ�����·�ʽ�Զ��嵱ǰ��Ҫ���õĻ������
                        //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//��ֵ��
                        //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet
                    }
                    //������ﲻ����Redis�������ã���Ŀǰ����Ĭ��ʹ���ڴ滺�� 

                    #endregion                        // DPBMARK_END

                    #region ���ú�ʹ�� Memcached      -- DPBMARK Memcached

                    //����Memcached���棨���裬������
                    if (UseMemcached(senparcSetting.Value, out string memcachedConfigurationStr)) //����Ϊ�˷��㲻ͬ�����Ŀ����߽������ã��������жϵķ�ʽ��ʵ�ʿ�������һ����ȷ���ģ������if�������Ժ���
                    {
                        app.UseEnyimMemcached();

                        /* ˵����
                        * 1��Memcached �������ַ�����Ϣ��� Config.SenparcSetting.Cache_Memcached_Configuration �Զ���ȡ��ע�ᣬ�粻��Ҫ�޸ģ��·��������Ժ���
                    /* 2�������ֶ��޸ģ�����ͨ���·� SetConfigurationOption �����ֶ����� Memcached ������Ϣ�����޸����ã����������ã�
                        */
                        Senparc.CO2NET.Cache.Memcached.Register.SetConfigurationOption(memcachedConfigurationStr);

                        //���»�������ȫ�ֻ�������Ϊ Memcached
                        Senparc.CO2NET.Cache.Memcached.Register.UseMemcachedNow();

                        //Ҳ����ͨ�����·�ʽ�Զ��嵱ǰ��Ҫ���õĻ������
                        CacheStrategyFactory.RegisterObjectCacheStrategy(() => MemcachedObjectCacheStrategy.Instance);
                    }

                    #endregion                        //  DPBMARK_END

                    #endregion

                    #region ע����־�����裬���飩

                    globalRegister.RegisterTraceLog(ConfigTraceLog);//����TraceLog

                    #endregion

                    #region APM ϵͳ����״̬ͳ�Ƽ�¼����

                    //����APM�������ʱ�䣨Ĭ������¿��Բ������ã�
                    CO2NET.APM.Config.EnableAPM = true;//Ĭ���Ѿ�Ϊ����������Ҫ�رգ�������Ϊ false
                    CO2NET.APM.Config.DataExpire = TimeSpan.FromMinutes(60);

                    #endregion

                    #endregion
                }, true)
                //ʹ�� Senparc.Weixin SDK
                .UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
                {
                    #region ΢���������

                    /* ΢�����ÿ�ʼ
                    * 
                    * ���鰴������˳�����ע�ᣬ�����뽫������ڵ�һλ��
                    */

                    #region ΢�Ż��棨���裬����������ÿ�ͷ����ȷ���������������������ע�����ʹ����ȷ�����ã�

                    //΢�ŵ� Redis ���棬�����ʹ����ע�͵������ǰ���뱣֤������Ч��������״��         -- DPBMARK Redis
                    if (UseRedis(senparcSetting.Value, out _))
                    {
                        app.UseSenparcWeixinCacheRedis();
                    }                                                                                     // DPBMARK_END

                    // ΢�ŵ� Memcached ���棬�����ʹ����ע�͵������ǰ���뱣֤������Ч��������״��    -- DPBMARK Memcached
                    if (UseMemcached(senparcSetting.Value, out _))
                    {
                        app.UseSenparcWeixinCacheMemcached();
                    }                                                                                      // DPBMARK_END

                    #endregion

                    #region ע�ṫ�ںŻ�С���򣨰��裩

                    //ע�ṫ�ںţ���ע������                                                    -- DPBMARK MP
                    weixinRegister
                            .RegisterMpAccount(senparcWeixinSetting.Value, "��ʢ������С���֡����ں�")// DPBMARK_END


                            //ע�������ںŻ�С���򣨿�ע������                                        -- DPBMARK MiniProgram
                            .RegisterWxOpenAccount(senparcWeixinSetting.Value, "��ʢ������С���֡�С����")// DPBMARK_END

                            //�������⣬��Ȼ�����ڳ�������ط�ע�ṫ�ںŻ�С����
                            //AccessTokenContainer.Register(appId, appSecret, name);//�����ռ䣺Senparc.Weixin.MP.Containers
                    #endregion

                    #region ע����ҵ�ţ����裩           -- DPBMARK Work

                            //ע����ҵ΢�ţ���ע������
                            .RegisterWorkAccount(senparcWeixinSetting.Value, "��ʢ�����硿��ҵ΢��")

                            //�������⣬��Ȼ�����ڳ�������ط�ע����ҵ΢�ţ�
                            //AccessTokenContainer.Register(corpId, corpSecret, name);//�����ռ䣺Senparc.Weixin.Work.Containers
                    #endregion                          // DPBMARK_END

                    #region ע��΢��֧�������裩        -- DPBMARK TenPay

                            //ע���΢��֧���汾��V2������ע������
                            .RegisterTenpayOld(senparcWeixinSetting.Value, "��ʢ������С���֡����ں�")//����� name �͵�һ�� RegisterMpAccount() �е�һ�£��ᱻ��¼��ͬһ�� SenparcWeixinSettingItem ������

                            //ע������΢��֧���汾��V3������ע������
                            .RegisterTenpayV3(senparcWeixinSetting.Value, "��ʢ������С���֡����ں�")//��¼��ͬһ�� SenparcWeixinSettingItem ������

                    #endregion                          // DPBMARK_END

                    #region ע��΢�ŵ�����ƽ̨�����裩  -- DPBMARK Open

                            //ע�������ƽ̨����ע������
                            .RegisterOpenComponent(senparcWeixinSetting.Value,
                                //getComponentVerifyTicketFunc
                                async componentAppId =>
                                {
                                    var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/OpenTicket"));
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    var file = Path.Combine(dir, string.Format("{0}.txt", componentAppId));
                                    using (var fs = new FileStream(file, FileMode.Open))
                                    {
                                        using (var sr = new StreamReader(fs))
                                        {
                                            var ticket = await sr.ReadToEndAsync();
                                            return ticket;
                                        }
                                    }
                                },

                            //getAuthorizerRefreshTokenFunc
                            async (componentAppId, auhtorizerId) =>
                            {
                                var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                                if (!Directory.Exists(dir))
                                {
                                    Directory.CreateDirectory(dir);
                                }

                                var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                                if (!File.Exists(file))
                                {
                                    return null;
                                }

                                using (Stream fs = new FileStream(file, FileMode.Open))
                                {
                                    var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                    var result = (RefreshAuthorizerTokenResult)binFormat.Deserialize(fs);
                                    return result.authorizer_refresh_token;
                                }
                            },

                                //authorizerTokenRefreshedFunc
                                (componentAppId, auhtorizerId, refreshResult) =>
                                {
                                    var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                                    using (Stream fs = new FileStream(file, FileMode.Create))
                                    {
                                        //���������������ʵ����ֻ��RefreshTokenҲ���ԣ�����RefreshToken����ˢ�µ����µ�AccessToken
                                        var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                        binFormat.Serialize(fs, refreshResult);
                                        fs.Flush();
                                    }
                                }, "��ʢ�����硿����ƽ̨")

                        //�������⣬��Ȼ�����ڳ�������ط�ע�Ὺ��ƽ̨��
                        //ComponentContainer.Register();//�����ռ䣺Senparc.Weixin.Open.Containers
                    #endregion                          // DPBMARK_END

                        ;

                    /* ΢�����ý��� */

                    #endregion
                });

            #region ʹ�� MessageHadler �м��������ȡ������������ Controller
            //MessageHandler �м�����ܣ�https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html

            //ʹ�ù��ںŵ� MessageHandler �м����������Ҫ���� Controller��                       --DPBMARK MP
            app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
            {
                //˵�����˴��������ʾ�˽�Ϊȫ��Ĺ��ܵ㣬�򻯵�ʹ�ÿ��Բο�����С�������ҵ΢��

                #region ���� SenparcWeixinSetting ���������Զ��ṩ Token��EncodingAESKey �Ȳ���

                //�˴�Ϊί�У����Ը���������̬�ж��������������룩
                options.AccountSettingFunc = context =>
                //����һ��ʹ��Ĭ������
                    senparcWeixinSetting.Value;

                //��������ʹ��ָ�����ã�
                //Config.SenparcWeixinSetting["<Your SenparcWeixinSetting's name filled with Token, AppId and EncodingAESKey>"]; 

                //����������� context ������̬�жϷ���Settingֵ

                #endregion

                //�� MessageHandler ���첽����δ�ṩ��дʱ������ͬ�����������裩
                options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;

                //�Է����쳣���д������ѡ��
                options.AggregateExceptionCatch = ex =>
                {
                    //�߼�����...
                    return false;//ϵͳ�����׳��쳣
                };
            });                                                                                   // DPBMARK_END

            //ʹ�� С���� MessageHandler �м��                                                   // -- DPBMARK MiniProgram
            app.UseMessageHandlerForWxOpen("/WxOpenAsync", CustomWxOpenMessageHandler.GenerateMessageHandler, options =>
                {
                    options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;
                    options.AccountSettingFunc = context => senparcWeixinSetting.Value;
                }
            );                                                                                    // DPBMARK_END

            //ʹ�� ��ҵ΢�� MessageHandler �м��                                                 // -- DPBMARK Work
            app.UseMessageHandlerForWork("/WorkAsync", WorkCustomMessageHandler.GenerateMessageHandler,
                                         o => o.AccountSettingFunc = c => senparcWeixinSetting.Value);//��򻯵ķ�ʽ
                                                                                                      // DPBMARK_END

            #endregion
        }


        /// <summary>
        /// ����΢�Ÿ�����־����ʾ�����裩
        /// </summary>
        private void ConfigTraceLog()
        {
            //������ΪDebug״̬ʱ��/App_Data/WeixinTraceLog/Ŀ¼�»�������־�ļ���¼���е�API������־����ʽ�����汾����ر�

            //���ȫ�ֵ�IsDebug��Senparc.CO2NET.Config.IsDebug��Ϊfalse���˴����Ե�������true�������Զ�Ϊtrue
            CO2NET.Trace.SenparcTrace.SendCustomLog("ϵͳ��־", "ϵͳ���");//ֻ��Senparc.Weixin.Config.IsDebug = true���������Ч

            //ȫ���Զ�����־��¼�ص�
            CO2NET.Trace.SenparcTrace.OnLogFunc = () =>
            {
                //����ÿ�δ���Log����Ҫִ�еĴ���
            };

            //����������WeixinException���쳣ʱ����
            WeixinTrace.OnWeixinExceptionFunc = async ex =>
            {
                //����ÿ�δ���WeixinExceptionLog����Ҫִ�еĴ���

                //����ģ����Ϣ������Ա                             -- DPBMARK Redis
                var eventService = new Senparc.Weixin.MP.Sample.CommonService.EventService();
                await eventService.ConfigOnWeixinExceptionFunc(ex);      // DPBMARK_END
            };
        }

        /// <summary>
        /// �жϵ�ǰ�����Ƿ�����ʹ�� Redis�������Ƿ��Ѿ��޸���Ĭ�������ַ����жϣ�
        /// </summary>
        /// <param name="senparcSetting"></param>
        /// <returns></returns>
        private bool UseRedis(SenparcSetting senparcSetting, out string redisConfigurationStr)
        {
            redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*Ĭ��ֵ��������*/;
            return useRedis;
        }

        /// <summary>
        /// �����жϵ�ǰ�����Ƿ�����ʹ�� Memcached�������Ƿ��Ѿ��޸���Ĭ�������ַ����жϣ�
        /// </summary>
        /// <param name="senparcSetting"></param>
        /// <returns></returns>
        private bool UseMemcached(SenparcSetting senparcSetting, out string memcachedConfigurationStr)
        {
            memcachedConfigurationStr = senparcSetting.Cache_Memcached_Configuration;
            var useMemcached = !string.IsNullOrEmpty(memcachedConfigurationStr) && memcachedConfigurationStr != "#{Cache_Memcached_Configuration}#";
            return useMemcached;
        }
    }
}
