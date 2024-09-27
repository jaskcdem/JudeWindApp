﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DataAcxess.ProjectContext
{
    public partial class ProjectContext
    {
        private IProjectContextProcedures _procedures;

        public virtual IProjectContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new ProjectContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IProjectContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<usp_GetSequenceResult>().HasNoKey().ToView(null);
        }
    }

    public partial class ProjectContextProcedures : IProjectContextProcedures
    {
        private readonly ProjectContext _context;

        public ProjectContextProcedures(ProjectContext context)
        {
            _context = context;
        }

        public virtual async Task<List<usp_GetSequenceResult>> usp_GetSequenceAsync(string SeqName, string Seq, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SeqName",
                    Size = 50,
                    Value = SeqName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Seq",
                    Size = 50,
                    Value = Seq ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<usp_GetSequenceResult>("EXEC @returnValue = [dbo].[usp_GetSequence] @SeqName, @Seq", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
