using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Web.Data
{
    public partial class AsesoriaContext : DbContext
    {
        public AsesoriaContext()
        {
        }

        public AsesoriaContext(DbContextOptions<AsesoriaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actividad> Actividads { get; set; }
        public virtual DbSet<Agente> Agentes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Auditorium> Auditoria { get; set; }
        public virtual DbSet<CategoriaTicket> CategoriaTickets { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClienteCuenta> ClienteCuentas { get; set; }
        public virtual DbSet<ClienteDireccione> ClienteDirecciones { get; set; }
        public virtual DbSet<ClienteEmail> ClienteEmails { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<EstadosTicket> EstadosTickets { get; set; }
        public virtual DbSet<FormasPago> FormasPagos { get; set; }
        public virtual DbSet<Mandato> Mandatos { get; set; }
        public virtual DbSet<NotaArchivo> NotaArchivos { get; set; }
        public virtual DbSet<Paise> Paises { get; set; }
        public virtual DbSet<PrioridadTicket> PrioridadTickets { get; set; }
        public virtual DbSet<TickectNota> TickectNotas { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TipoCliente> TipoClientes { get; set; }
        public virtual DbSet<TipoIdentificacionFiscal> TipoIdentificacionFiscals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Actividad>(entity =>
            {
                entity.HasKey(e => e.IdActividad)
                    .HasName("PK_Table_1");

                entity.ToTable("Actividad");

                entity.Property(e => e.Descripcion).HasMaxLength(500);
            });

            modelBuilder.Entity<Agente>(entity =>
            {
                entity.HasKey(e => e.IdAgente);

                entity.Property(e => e.Agente1)
                    .HasMaxLength(300)
                    .HasColumnName("Agente");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.ToTable("AspNetUserLogin");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.ToTable("AspNetUserToken");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Auditorium>(entity =>
            {
                entity.HasKey(e => e.IdAuditoria);

                entity.Property(e => e.Accion).HasMaxLength(500);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Usuario).HasMaxLength(100);
            });

            modelBuilder.Entity<CategoriaTicket>(entity =>
            {
                entity.HasKey(e => e.IdCategoriaTicket);

                entity.ToTable("CATEGORIA_TICKET");

                entity.Property(e => e.IdCategoriaTicket).HasColumnName("ID_CATEGORIA_TICKET");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CodigoCliente);

                entity.ToTable("CLIENTES");

                entity.Property(e => e.CodigoCliente).HasColumnName("CODIGO_CLIENTE");

                entity.Property(e => e.AceptaFacturaElectronica).HasColumnName("ACEPTA_FACTURA_ELECTRONICA");

                entity.Property(e => e.Agente).HasColumnName("AGENTE");

                entity.Property(e => e.CesionDatos).HasColumnName("CESION_DATOS");

                entity.Property(e => e.CodigoContabilidad).HasColumnName("CODIGO_CONTABILIDAD");

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(50)
                    .HasColumnName("CODIGO_POSTAL");

                entity.Property(e => e.CodigoProveedor)
                    .HasMaxLength(100)
                    .HasColumnName("CODIGO_PROVEEDOR");

                entity.Property(e => e.CrearRecibo).HasColumnName("CREAR_RECIBO");

                entity.Property(e => e.CuentaContableTresDigitos)
                    .HasMaxLength(3)
                    .HasColumnName("CUENTA_CONTABLE_TRES_DIGITOS");

                entity.Property(e => e.DireccionWeb)
                    .HasMaxLength(100)
                    .HasColumnName("DIRECCION_WEB");

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(500)
                    .HasColumnName("DOMICILIO");

                entity.Property(e => e.EnviooComunicaciones).HasColumnName("ENVIOO_COMUNICACIONES");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("FAX");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ALTA");

                entity.Property(e => e.IdActividad).HasColumnName("ID_ACTIVIDAD");

                entity.Property(e => e.IdFormaPago).HasColumnName("ID_FORMA_PAGO");

                entity.Property(e => e.IdIdentificacionFiscal).HasColumnName("ID_IDENTIFICACION_FISCAL");

                entity.Property(e => e.IdPais).HasColumnName("ID_PAIS");

                entity.Property(e => e.IdTipoCliente).HasColumnName("ID_TIPO_CLIENTE");

                entity.Property(e => e.IdentificacionFiscal)
                    .HasMaxLength(200)
                    .HasColumnName("IDENTIFICACION_FISCAL");

                entity.Property(e => e.Iva).HasColumnName("IVA");

                entity.Property(e => e.MensajeEmergente)
                    .HasMaxLength(300)
                    .HasColumnName("MENSAJE_EMERGENTE");

                entity.Property(e => e.Modificado)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICADO");

                entity.Property(e => e.Movil)
                    .HasMaxLength(50)
                    .HasColumnName("MOVIL");

                entity.Property(e => e.NoFacturas).HasColumnName("NO_FACTURAS");

                entity.Property(e => e.NoImprimirEnListados).HasColumnName("NO_IMPRIMIR_EN_LISTADOS");

                entity.Property(e => e.NoVender).HasColumnName("NO_VENDER");

                entity.Property(e => e.NombreComercial)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE_COMERCIAL");

                entity.Property(e => e.NombreFiscal)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE_FISCAL");

                entity.Property(e => e.Observaciones).HasColumnName("OBSERVACIONES");

                entity.Property(e => e.PersonaContacto)
                    .HasMaxLength(50)
                    .HasColumnName("PERSONA_CONTACTO");

                entity.Property(e => e.Poblacion)
                    .HasMaxLength(50)
                    .HasColumnName("POBLACION");

                entity.Property(e => e.Provincia)
                    .HasMaxLength(50)
                    .HasColumnName("PROVINCIA");

                entity.Property(e => e.Recargo).HasColumnName("RECARGO");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .HasColumnName("TELEFONO");

                entity.HasOne(d => d.AgenteNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.Agente)
                    .HasConstraintName("FK_CLIENTES_Agentes");

                entity.HasOne(d => d.IdActividadNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdActividad)
                    .HasConstraintName("FK_CLIENTES_Actividad");

                entity.HasOne(d => d.IdFormaPagoNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdFormaPago)
                    .HasConstraintName("FK_CLIENTES_FormasPago");

                entity.HasOne(d => d.IdIdentificacionFiscalNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdIdentificacionFiscal)
                    .HasConstraintName("FK_CLIENTES_TIPO_IDENTIFICACION_FISCAL");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdPais)
                    .HasConstraintName("FK_CLIENTES_PAISES");

                entity.HasOne(d => d.IdTipoClienteNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdTipoCliente)
                    .HasConstraintName("FK_CLIENTES_TipoCliente");
            });

            modelBuilder.Entity<ClienteCuenta>(entity =>
            {
                entity.HasKey(e => e.IdCuenta);

                entity.ToTable("CLIENTE_CUENTAS");

                entity.Property(e => e.IdCuenta).HasColumnName("ID_CUENTA");

                entity.Property(e => e.Activa).HasColumnName("ACTIVA");

                entity.Property(e => e.Banco)
                    .HasMaxLength(100)
                    .HasColumnName("BANCO");

                entity.Property(e => e.Bic)
                    .HasMaxLength(50)
                    .HasColumnName("BIC");

                entity.Property(e => e.Ccc)
                    .HasMaxLength(200)
                    .HasColumnName("CCC");

                entity.Property(e => e.Iban)
                    .HasMaxLength(200)
                    .HasColumnName("IBAN");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdMandato).HasColumnName("ID_MANDATO");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.ClienteCuenta)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_CLIENTE_CUENTAS_CLIENTES");

                entity.HasOne(d => d.IdMandatoNavigation)
                    .WithMany(p => p.ClienteCuenta)
                    .HasForeignKey(d => d.IdMandato)
                    .HasConstraintName("FK_CLIENTE_CUENTAS_MANDATOS");
            });

            modelBuilder.Entity<ClienteDireccione>(entity =>
            {
                entity.HasKey(e => e.IdDireccion);

                entity.ToTable("CLIENTE_DIRECCIONES");

                entity.Property(e => e.IdDireccion).HasColumnName("ID_DIRECCION");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.ClienteDirecciones)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_CLIENTE_DIRECCIONES_CLIENTES");
            });

            modelBuilder.Entity<ClienteEmail>(entity =>
            {
                entity.HasKey(e => e.IdEmailCliente);

                entity.ToTable("CLIENTE_EMAILS");

                entity.Property(e => e.IdEmailCliente).HasColumnName("ID_EMAIL_CLIENTE");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.ClienteEmails)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_CLIENTE_EMAILS_CLIENTES");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa)
                    .HasName("PK_Empresas");

                entity.ToTable("Empresa");

                entity.Property(e => e.Cif)
                    .HasMaxLength(50)
                    .HasColumnName("CIF");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.Logo).HasMaxLength(50);

                entity.Property(e => e.Logotipo).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.TelefonoFijo).HasMaxLength(50);

                entity.Property(e => e.TelefonoMovil).HasMaxLength(50);
            });

            modelBuilder.Entity<EstadosTicket>(entity =>
            {
                entity.HasKey(e => e.IdEstadoTicket);

                entity.ToTable("ESTADOS_TICKETS");

                entity.Property(e => e.IdEstadoTicket).HasColumnName("ID_ESTADO_TICKET");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<FormasPago>(entity =>
            {
                entity.HasKey(e => e.IdFomaPago);

                entity.ToTable("FormasPago");

                entity.Property(e => e.Descripcion).HasMaxLength(100);
            });

            modelBuilder.Entity<Mandato>(entity =>
            {
                entity.HasKey(e => e.IdMandato);

                entity.ToTable("MANDATOS");

                entity.Property(e => e.IdMandato).HasColumnName("ID_MANDATO");

                entity.Property(e => e.Activo).HasColumnName("ACTIVO");

                entity.Property(e => e.FechaFirma)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_FIRMA");

                entity.Property(e => e.ReferenciaUnica)
                    .HasMaxLength(500)
                    .HasColumnName("REFERENCIA_UNICA");
            });

            modelBuilder.Entity<NotaArchivo>(entity =>
            {
                entity.HasKey(e => e.IdNotaArchivos);

                entity.ToTable("NOTA_ARCHIVOS");

                entity.Property(e => e.IdNotaArchivos).HasColumnName("ID_NOTA_ARCHIVOS");

                entity.Property(e => e.IdNota).HasColumnName("ID_NOTA");

                entity.Property(e => e.RutaArchivo).HasColumnName("RUTA_ARCHIVO");

                entity.HasOne(d => d.IdNotaNavigation)
                    .WithMany(p => p.NotaArchivos)
                    .HasForeignKey(d => d.IdNota)
                    .HasConstraintName("FK_NOTA_ARCHIVOS_TICKECT_NOTAS");
            });

            modelBuilder.Entity<Paise>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.ToTable("PAISES");

                entity.Property(e => e.IdPais).HasColumnName("ID_PAIS");

                entity.Property(e => e.Claim)
                    .HasMaxLength(50)
                    .HasColumnName("CLAIM");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<PrioridadTicket>(entity =>
            {
                entity.HasKey(e => e.IdPrioridad);

                entity.ToTable("PRIORIDAD_TICKET");

                entity.Property(e => e.IdPrioridad).HasColumnName("ID_PRIORIDAD");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(10)
                    .HasColumnName("DESCRIPCION")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TickectNota>(entity =>
            {
                entity.HasKey(e => e.IdNota);

                entity.ToTable("TICKECT_NOTAS");

                entity.Property(e => e.IdNota).HasColumnName("ID_NOTA");

                entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.IdTicket).HasColumnName("ID_TICKET");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(450)
                    .HasColumnName("USUARIO");

                entity.HasOne(d => d.IdTicketNavigation)
                    .WithMany(p => p.TickectNota)
                    .HasForeignKey(d => d.IdTicket)
                    .HasConstraintName("FK_TICKECT_NOTAS_TICKETS");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.TickectNota)
                    .HasForeignKey(d => d.Usuario)
                    .HasConstraintName("FK_TICKECT_NOTAS_AspNetUsers");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.IdTicket);

                entity.ToTable("TICKETS");

                entity.Property(e => e.IdTicket).HasColumnName("ID_TICKET");

                entity.Property(e => e.AsignadoUsuario)
                    .HasMaxLength(450)
                    .HasColumnName("ASIGNADO_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(450)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.FechaActualizado)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ACTUALIZADO");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CREADO");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_VENCIMIENTO");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.IdPrioridad).HasColumnName("ID_PRIORIDAD");

                entity.Property(e => e.IdSeguimiento).HasColumnName("ID_SEGUIMIENTO");

                entity.Property(e => e.Respuestas).HasColumnName("RESPUESTAS");

                entity.Property(e => e.TiempoDedicado)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("TIEMPO_DEDICADO");

                entity.Property(e => e.UltimaRespuesta)
                    .HasMaxLength(100)
                    .HasColumnName("ULTIMA_RESPUESTA");

                entity.HasOne(d => d.AsignadoUsuarioNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.AsignadoUsuario)
                    .HasConstraintName("FK_TICKETS_AspNetUsers");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_TICKETS_CATEGORIA_TICKET");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_TICKETS_CLIENTES");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK_TICKETS_ESTADOS_TICKETS");

                entity.HasOne(d => d.IdPrioridadNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.IdPrioridad)
                    .HasConstraintName("FK_TICKETS_PRIORIDAD_TICKET");
            });

            modelBuilder.Entity<TipoCliente>(entity =>
            {
                entity.HasKey(e => e.IdTipoCliente);

                entity.ToTable("TipoCliente");

                entity.Property(e => e.TipoCliente1)
                    .HasMaxLength(100)
                    .HasColumnName("TipoCliente");
            });

            modelBuilder.Entity<TipoIdentificacionFiscal>(entity =>
            {
                entity.HasKey(e => e.IdTipoIdentificacionFiscal);

                entity.ToTable("TIPO_IDENTIFICACION_FISCAL");

                entity.Property(e => e.IdTipoIdentificacionFiscal).HasColumnName("ID_TIPO_IDENTIFICACION_FISCAL");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .HasColumnName("DESCRIPCION");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
