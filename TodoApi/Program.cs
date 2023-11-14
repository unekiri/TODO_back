// WebAPIアプリケーションを実行可能な状態にし、HTTPリクエストを処理するための基本的な設定を行う

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using TodoApi.Models;

//Cross-Origin-Resource-Sharingの名前を指定する変数の宣言(CORSポリシーは、クライアントAPPからのHTTPリクエストを受け入れるために使用される)
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Webアプリケーションのビルダー(骨組み)を作成(ビルダーを使って、アプリケーションの構成やサービスの設定を行う)
var builder = WebApplication.CreateBuilder(args);

// CORSポリシーを構成(任意のヘッダーとHTTPメソッドを許可する)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: MyAllowSpecificOrigins,
                  policy =>
                  {
                      policy.AllowAnyOrigin()  // 全てのオリジン(クロスオリジンのHTTPリクエスト元)を許可する
                            .AllowAnyHeader()  // 全てのヘッダーを許可する
                            .AllowAnyMethod(); // 全てのHTTPメソッドを許可する
                  });
});

// services.AddResponseCashing();

// コントローラーをAPPに追加する
builder.Services.AddControllers();

// EFCoreを使って、DBコンテキストをAPPに追加する(DB接続文字列を構成から取得して、DBにアクセスできるようにする)
builder.Services.AddDbContext<TodoContext>(opt =>
   　　　　　　　　// appsettings.json(アプリケーションの構成ファイル)から'TodoContext'という名前の接続文字列を取得する
   opt.UseNpgsql(builder.Configuration.GetConnectionString("TodoContext") ?? throw new InvalidOperationException("Connection string 'TodooContext' not found.")));


// アプリケーションを構築し、設定を適用した後に実行可能な状態にする
var app = builder.Build();

// アプリケーションが開発環境で実行されている場合のみ、開発者向けのエラーページを表示する
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// CORSポリシーをアプリケーションに適用する(クライアントからのHTTPリクエストの許可)
app.UseCors(MyAllowSpecificOrigins);

// 静的ファイルサービスを有効にし、デフォルトのファイル(index.html)を提供を可能にする
app.UseDefaultFiles();
app.UseStaticFiles();

// HTTPSへのリダイレクトを可能にする(HTTPリクエストがあった際に、HTTPS接続にリダイレクトする)
app.UseHttpsRedirection();

// 認証ミドルウェアを有効にする(ミドルウェア:OSとアプリケーションの中間で頑張るプログラム)
app.UseAuthorization();

// コントローラーに対するHTTPリクエストのマッピング(関連付け)を設定する(コントローラーのアクションが呼び出され、応答が生成される)
app.MapControllers();

// アプリケーションを実行し、HTTPリクエストを待ち受けて応答を生成するメインのエントリーポイント(アプリケーションが実行されると、指定されたポートでHTTPリクエストを受け入れる)
app.Run();