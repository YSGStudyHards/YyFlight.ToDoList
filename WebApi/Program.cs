using Microsoft.OpenApi.Models;
using Repository.Interface;
using Repository;
using System.Reflection;
using Application.User;
using Repository.Repositories.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 添加Swagger服务
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDoList API",
        Version = "V1",
        Description = ".NET7使用MongoDB开发ToDoList系统",
        Contact = new OpenApiContact
        {
            Name = "GitHub源码地址",
            Url = new Uri("https://github.com/YSGStudyHards/YyFlight.ToDoList")
        }
    });

    // 获取xml文件名
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // 获取xml文件路径
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // 添加控制器层注释，true表示显示控制器注释
    options.IncludeXmlComments(xmlPath, true);
    // 对action的名称进行排序，如果有多个，就可以看见效果了
    options.OrderActionsBy(o => o.RelativePath);
});

//注册数据库基础操作和工作单元
builder.Services.AddSingleton<IMongoConnection, MongoConnection>();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//注册相关应用服务
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUserOperationExampleServices, UserOperationExampleServices>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//使中间件能够将生成的Swagger用作JSON端点.
app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
//允许中间件为Swagger UI（HTML、JS、CSS等）提供服务，指定swagger JSON端点.
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
