using CodeReview.Business.Models.Fornecedores;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeReview.Infra.Data.Mappings
{
    public class FornecedorConfig : EntityTypeConfiguration<Fornecedor>
    {
            public FornecedorConfig()
        {
            HasKey(f => f.Id);

            Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(200);

            Property(f => f.Documento)
                .IsRequired()
                .HasMaxLength(14)
                .HasColumnAnnotation("Index_Documento", new IndexAnnotation(new IndexAttribute { IsUnique = true }))
                .IsFixedLength();

            HasRequired(f => f.Endereco)
                .WithRequiredPrincipal(f => f.fornecedor);

            ToTable("Fornecedores");
        }
    }
}
