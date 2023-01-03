using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMvc();

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

    //// 获取xml文件名
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //// 获取xml文件路径
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //// 添加控制器层注释，true表示显示控制器注释
    //options.IncludeXmlComments(xmlPath, true);
    ////c.IncludeXmlComments(path, true); // true : 显示控制器层注释
    //options.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序，如果有多个，就可以看见效果了。

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();


// Configure the HTTP request pipeline.


// 添加Swagger相关中间件
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseStaticFiles();


app.Run();
