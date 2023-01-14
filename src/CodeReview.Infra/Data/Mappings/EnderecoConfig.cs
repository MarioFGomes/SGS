using CodeReview.Business.Models.Fornecedores;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Infra.Data.Mappings
{
    public class EnderecoConfig : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfig()
        {
            HasKey(e => e.Id);

            Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(200);

            Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(200);

            Property(e => e.Numero);

            Property(e => e.Rua)
                .IsRequired()
                .HasMaxLength(200);

            Property(e=>e.Morada)
                .HasMaxLength(200);

            Property(e => e.Municipio)
                .IsRequired()
                .HasMaxLength(200);

            ToTable("Endereços");

                
        }

    }
}
